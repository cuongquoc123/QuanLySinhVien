namespace QuanLySinhVien.MidWare.Filter
{
    public class GlabalLogger
    {
        private readonly ILogger<GlabalLogger> _logger;
        private static readonly Dictionary<string, int> APITotalCounter = new();

        private static readonly Dictionary<string, List<DateTime>> IpRequestLog = new();

        private readonly RequestDelegate _next;

        private const int LIMIT = 100;

        public GlabalLogger(RequestDelegate next, ILogger<GlabalLogger> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.ToString();
            
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            var now = DateTime.Now;

            //Đếm số lần API được gọi
            if (APITotalCounter.ContainsKey(path))
            {
                APITotalCounter[path]++;
            }
            else
            {
                APITotalCounter[path] = 1;
            }

            if (!IpRequestLog.ContainsKey(ip))
            {
                IpRequestLog[ip] = new List<DateTime>();
            }

            IpRequestLog[ip].Add(now);

            IpRequestLog[ip] = IpRequestLog[ip].
                Where(t => (now - t).TotalHours <= 1).ToList();

            int countLastHour = IpRequestLog[ip].Count;

            if (countLastHour > LIMIT)
            {
                throw new CustomError(429,"Rate Limit","Rate limit exceeded. Try again later.");
            }

            int totalCall = APITotalCounter[path];

            var start = DateTime.Now;
            await _next(context);
            var elapse = DateTime.Now - start;

            _logger.LogInformation($"API: {path}; User: {ip};Proccess Time: {elapse}ms; Call (1h): {countLastHour}; Total call: {totalCall}");
        }
    }
}