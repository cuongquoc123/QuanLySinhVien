--Tạo CSDL 
use master;
Drop Database if exists ql_cua_hang;
create Database ql_cua_hang
on primary 
(
	Name = 'ql_cua_hang_Primary',
	filename = 'D:\csdl\CSDLDA\ql_cua_hang_Primary.mdf',
	size = 30mb
)
log on 
(
	name = 'ql_cua_hang_log',
	filename = 'D:\csdl\CSDLDA\ql_cua_hang_log.ldf'
);
go

use ql_cua_hang;

drop table if exists cuahang;
create table cuahang(
	CuaHangId char(10) primary key,
	TenCH nvarchar(50) not null,
	DiaChi nvarchar(50) not null,	
	SDT char(11),
	statusS nvarchar(100),
	Email varchar(100)
);
go

drop table if exists staff;
create table staff(
	StaffId char(10) primary key,
	CCCD char(11) not null,
	Ten nvarchar(50) not null,
	DiaChi nvarchar(50) not null,
	NgaySinh date,
	Vtri nvarchar(50),
	Luong money check(Luong > 0),
	Thuong money,
	Avatar nvarchar(500) not null Default('https://bla.edu.vn/wp-content/uploads/2025/09/avatar-fb.jpg'),
	StatuSf nvarchar(100),
	CuaHangId char(10),
	Constraint FK_CH_STAFF foreign key (CuaHangId) references cuahang (CuaHangId) 
)
go

drop table if exists nguyenlieu;
create table nguyenlieu (
	MaNguyenLieu char(10) primary key,
	TenNguyenLieu nvarchar(50) not null,
	DVT nvarchar(20) not null
);
go

drop table if exists kho;
create table kho (
	MaKho char(10) primary key,
	TrangThai nvarchar(50),
	DiaChi nvarchar(50),
	CuaHangId char(10) not null,
	Constraint FK_Kho_CH foreign key (CuaHangId) references cuahang(CuaHangId)
);
go
drop table if exists TonKho;
create table TonKho(
	MaKho char(10) not null,
	Constraint FK_Kho_TonKho foreign key (MaKho) references kho(MaKho),
	MaNguyenLieu char(10) not null,
	Constraint FK_Kho_NguyenLieu foreign key (MaNguyenLieu) references nguyenlieu(MaNguyenLieu),
	SoLuongTon int check (SoLuongTon >= 0) not null
);
go
drop table if exists phieu_nhapNL; 
create table phieu_nhapNL(
	MaPhieu char(10) primary key,
	ngay_nhap datetime2 not null,
	MaKho char(10),
	Constraint FK_CH_PNL foreign key (MaKho) references kho(MaKho) 
);
go



drop table if exists chi_tiet_phieu_nhap;
create table chi_tiet_phieu_nhap (
	MaNguyenLieu char(10) not null, 
	SoLuong int Check(SoLuong > 0),
	MaPhieu char(10) not null,
	Primary key (MaNguyenLieu,MaPhieu),
	Constraint FK_CTYC_PHIEU foreign key (MaPhieu) references phieu_nhapNL(MaPhieu),
	Constraint FK_CTYC_NL foreign key (MaNguyenLieu) references nguyenlieu(MaNguyenLieu)
);
go



drop table if exists sysrole;
create table sysrole(
	RoleId char(10) primary key,
	RoleName nvarchar(20) not null
); 
go

drop table if exists sysuser;
create table sysuser (
	UserId char(10) primary key,
	UserName nvarchar(100) unique not null,
	Passwords nvarchar(100) not null,
	RoleId char(10)
	Constraint FK_USER_ROLE foreign key (RoleId) references sysrole(RoleId),
	Constraint FK_USER_STAFF foreign key (UserId) references Staff(StaffId)
);
go

drop table if exists LoaiDanhMuc;
create table LoaiDanhMuc(
	MaLoaiDM char(10) primary key,
	TenLoaiDM nvarchar(10)
);
go

drop table if exists danhmuc;
create table danhmuc (
	maDM char(10) primary key,
	tenDM nvarchar(50) not null,
	MaLoaiDM char(10) not null,
	Constraint FK_DM_LDM foreign key (MaLoaiDM) references LoaiDanhMuc(MaLoaiDM)
);
go

drop table if exists sanpham;
create table sanpham (
	MaSP char(20) primary key,
	DonGia money not null check(DonGia > 0),
	TenSP nvarchar(50) not null,
	Anh nvarchar(500),
	status nvarchar(50),
	Mota text,
	maDM char(10) not null,
	Constraint FK_SP_DM foreign key (maDM) references danhmuc(maDM)
);
go

drop table if exists Customer;
create table Customer(
	CustomerId char(10) primary key,
	UserName nvarchar(50) unique not null,
	Passwords nvarchar(100) not null,
	statusC NVARCHAR(50)
);
go
drop table if exists CustomerDetail;
create table CustomerDetail(
	CustomerId char(10) primary key,
	TenKhach nvarchar(50),
	CCCD char(11),
	SDT char(10) unique,
	Email NVARCHAR(50) UNIQUE,
	Avatar nvarchar(500) default (N'https://bla.edu.vn/wp-content/uploads/2025/09/avatar-fb.jpg'),
	DiaChi nvarchar(100),
	constraint FK_Detail_Customer foreign key (CustomerId) references Customer(CustomerId)
);
go
drop table if exists donhang;
create table donhang(
	MaDon char(10) primary key,
	TrangThai nvarchar(50),
	NgayNhan Datetime2 not null,
	NgayHoangThanh datetime2,
	CustomerId char(10),
	Constraint FK_Cus_DH foreign key (CustomerId) references Customer(CustomerId),
	UserId char(10), 
	Constraint FK_NV_DH foreign key (UserId) references sysuser(UserId)
); 
go

drop table if exists chi_tiet_don_hang;
create table chi_tiet_don_hang (
	SoLuong int not null check (SoLuong > 0),
	MaDon char(10) not null,
	MaSP char(20) not null, 
	primary key (MaDon,MaSP),
	Constraint FK_CTDH_SP foreign key (MaSP) references sanpham(MaSP),
	Constraint FK_CTDH_DH foreign key (MaDon) references donhang(MaDon)
);
go

set dateformat dmy;
INSERT into sysrole (RoleId,RoleName) VALUES
('R01','admin'),
('R02','manager'),
('R03','cashier');
insert into cuahang (CuaHangId,TenCH,DiaChi,statusS,SDT) values 
('CH01',N'Cửa hàng mặc định','Cửa hàng online','Đang hoạt động','0123125451');

insert into Customer (CustomerId,UserName,Passwords) values 
('CTM01',N'MĐ','123456467865453121432');
insert into CustomerDetail (CustomerId,TenKhach) values
('CTM01',N'Khách vãn lai');

insert into staff(StaffId,Ten,DiaChi,CCCD,NgaySinh,CuaHangId) values
('ST01','admin hệ thống','Onlive','012356547','01/01/1999','CH01');
insert into sysuser(UserId,UserName,Passwords) values
('ST01','admin1','123');
go

--Tạo Trigger 
--Trigger khi create bảng chi tiết yêu cầu sẽ cập nhật số lượng tồn kho của cửa hàng tương ứng
create trigger UpdateSoLuongTonKho on chi_tiet_phieu_nhap
after Insert 
as
begin
	set nocount on;
	Declare @MaNL char(10), @MaPhieu char(10), @MaKH char(10), @SoLuong int;
	Set @MaNL = (Select MaNguyenLieu from inserted)
	set @MaPhieu = (Select MaPhieu from inserted)
	Set @MaKH = (Select top 1 MaKho from phieu_nhapNL where MaPhieu = @MaPhieu)
	Set @SoLuong = (Select SoLuong from inserted)

	update TonKho set SoLuongTon = SoLuongTon + @SoLuong where MaKho = @MaKH and MaNguyenLieu = @MaNL
end;
go

--Trigger khi tạo 1 phiếu nhập nguyên liệu thì tư cài đặt ngày nhập là ngày hiện tại
create trigger TuDienCotNgayNhap  on phieu_nhapNL
instead of insert
as 
begin
	set nocount on;
	insert into phieu_nhapNL (MaPhieu,MaKho,ngay_nhap)
	select  i.MaPhieu, i.MaKho, CAST(GETDATE() as Datetime2) from inserted i
end;
go

--Trigger Kiểm tra của tuổi của staff khi thêm vào phải đủ 18 tuổi 
create trigger KiemTraTuoiStaff on staff
for insert
as
begin
	Declare @NgayHienTai Date = Cast(GetDate() as date), @NgaySinh date;
	set @NgaySinh = (select top 1 NgaySinh from inserted)

	Declare @tuoi int
	--Tính tuổi cho staff mới (18 tuổi phải đủ cả ngày lẫn tháng)
	set @tuoi = case 
					when MONTH(@NgayHienTai) < MONTH(@NgaySinh) then --Không đủ tháng 
						DATEDIFF(YEAR,@NgaySinh,@NgayHienTai) - 1 
					when MONTH(@NgayHienTai) = MONTH(@NgaySinh) and DAY(@NgayHienTai) < DAY(@NgaySinh) then --Không đủ ngày
						DATEDIFF(YEAR,@NgaySinh,@NgayHienTai) - 1 
					else
						DATEDIFF(YEAR,@NgaySinh,@NgayHienTai)
					end;
	if (@tuoi >= 18)
		commit tran
	else
		begin
			print 'Staff vừa thêm vào không đủ 18 tuổi'
			rollback tran
		end;
end;
go
--Trigger kiểm tra nếu cập nhật trạng thái là hoàn thành thì gán ngày giờ hoàn thành là ngày giờ hiện tại, Không phải hoàn thành thì set null
create trigger CapNhatGioHoanThanh on donhang
after update
as
begin
	SET NOCOUNT ON;
	IF not UPDATE(TrangThai)
	begin
		return
	end;
	
	update dh set  dh.NgayHoangThanh = case 
											when i.TrangThai = N'hoàn thành' then SYSDATETIME()
											else null 
										end
	from donhang dh
	join inserted i on dh.MaDon  = i.MaDon
										 
end;
go
--Trigger Instead of Delete trên bảng staff chỉ đổi trạng thái thành nghĩ việc thay vì xóa 
create trigger NganChanXoaStaff on staff
instead of Delete
as
begin
	set nocount on;

	update s
	set s.StatuSf = N'Nghỉ Việc'
	from staff s
	inner join deleted as d on s.StaffId = d.StaffId
end;
go
--trigger Không cho thêm sản phẩm mới vào các đơn có trạng thái ‘Đang xử lý’, ‘Hoàn thành’, ‘Đã hủy’
create trigger NganThemMoiSPChoDonXuLy on chi_tiet_don_hang
after insert
as
begin
	set nocount on;
	if Exists (select 1
				from inserted i
				inner join donhang dh on dh.MaDon = i.MaDon
				where dh.TrangThai in (N'Đang xử lý',N'Hoàn thành','Đã hủy'))
	begin
		print N'Đơn hàng này đã qua quá trình thêm mới sản phẩm'
		Rollback tran
		return
	end;
end;
go
--Proc của hệ thống
--	Proc tạo đơn hàng truyền vào CustomerId, StaffId và 
-- 1 mã sản phẩm cùng số lượng nếu CustomerId là rỗng hoặc null thì set CustomerId là ‘CTM01’ nếu StaffId là rỗng hoặc null thì Set là ‘ST01’
create type dbo.ChiTietType as table 
(
	Ma char(10) primary key,
	SoLuong int not null check(SoLuong >= 0)
);
go
create proc TaoDonHang @CustomerId char(10), @StaffId char(10), @MaDon char(10),@DanhSachSP dbo.ChiTietType readonly
as
begin
	set  nocount on;

	if not exists (select 1 from @DanhSachSP) or @MaDon is null
	begin
		print N'Phải có ít nhất 1 sản phẩm mới tạo đơn được hoặc chưa truyền mã đơn'
		return -1;
	end;
	declare @Customer char(10) = Isnull(Nullif(@CustomerId,''), 'CTM01');
	declare @Staff char(10) = isnull(Nullif(@StaffId,''),'ST01');

	begin tran
		begin try
			insert into donhang (MaDon,CustomerId,UserId,NgayNhan,TrangThai) values
			(@MaDon,@Customer,@Staff,SYSDATETIME(),N'Tiếp nhận')

			insert into chi_tiet_don_hang(MaDon,MaSP,SoLuong)
			select @MaDon, sps.Ma,sps.SoLuong from @DanhSachSP as sps

			return 0;
		end try
		begin catch
			rollback tran
			return -1;
		end catch

end;
go

--Proc Tạo mới Phiếu nhập nguyên liệu  và chi tiết phiếu nhập
create proc TaoPhieuNhap @MaPhieu char(10), @MaKho char(10),@DanhSachNL dbo.ChiTietType readonly
as
begin
	set nocount on;
	declare @MP  char(10)= NULLif(@MaPhieu,'');
	declare @MK char(10) = Nullif(@MaKho,'');
	if not exists (select 1 from @DanhSachNL) or @MP is null or @MK is null
	begin
		print N'Phải có danh sách nguyên liệu kèm số lượng mã kho và mã phiếu để tạo phiếu nhập'
		return -1;
	end;

	begin tran
		begin try
			insert into phieu_nhapNL(MaPhieu,MaKho,ngay_nhap) values 
			(@MaPhieu,@MaKho,SYSDATETIME());

			insert into chi_tiet_phieu_nhap(MaNguyenLieu,MaPhieu,SoLuong)
			select detail.Ma, @MaPhieu,detail.SoLuong
			from @DanhSachNL detail
			return 0
		end try
		begin catch
			rollback tran
			return -1;
		end catch
end;
go
--Proc tạo kho khi tạo 1 kho mới sẽ tự động tạo thêm tồn kho cho các nguyên liệu với số lượng là 0
create proc TaoKhoMoi @Makho char(10),@MaCH char(10),@DiaChi nvarchar(50)
as
begin
	set nocount on;
	
	if Nullif(@Makho,'') is null or NUllif(@MaCH,'') is null
	begin
		print N'Mã kho và mã cửa hàng không được để trống'
		return -1;
	end

	begin tran
		begin try
			-- tạo kho mới
			insert into kho (MaKho,DiaChi,CuaHangId,TrangThai) values
			(@Makho,@DiaChi,@MaCH,N'Hoạt động')
			--Tạo tồn kho với các nguyên liệu trong bảng nguyên liệu
			insert into TonKho(MaNguyenLieu,MaKho,SoLuongTon) 
			select MaNguyenLieu, @Makho, 0
			from nguyenlieu

			return 0;
		end try
		begin catch
			rollback tran 
			return -1;
		end catch

end;
go
--Proc tạo nguyên liệu sẽ tự động cập nhật tồn kho cho tất cả các kho với số lượng là 0
create proc TaoMoiNguyenLieu @MaNguyenLieu char(10), @DVT nvarchar(20), @TenNL nvarchar(50)
as
begin
	set NOCOUNT ON;
	if nullif(@MaNguyenLieu,'') is null or nullif(@DVT,'') is null or nullif(@TenNL,'') is null
	begin
		print 'Phải đủ các trường yêu cầu'
		return -1;
	end;

	begin tran
		begin try
			insert into nguyenlieu (MaNguyenLieu,TenNguyenLieu,DVT) values 
			(@MaNguyenLieu,@TenNL,@DVT);

			insert into TonKho(MaKho,MaNguyenLieu,SoLuongTon) 
			select MaKho,@MaNguyenLieu,0
			from kho
			return 0;
		end try
		begin catch
			rollback
			return -1
		end catch
end;
go
--proc tạo tài khoản cho staff (điều kiện là staff đã tồn tại ở bảng staff)
CREATE PROCEDURE TaoTaiKhoanStaff @StaffId CHAR(10), @UserName NVARCHAR(100), @PassWord NVARCHAR(100)
AS
BEGIN
	set nocount ON;
	IF not exists (select 1 from staff where StaffId = @StaffId)
	BEGIN
		PRINT N'Không thể tạo vì staff này không có thông tin trong hệ thống'
		RETURN -1;
	END
	if exists (select 1 from sysuser where UserName = @UserName)
	BEGIN
		print N'Tên người dùng đã tồn tại'
		RETURN -1;
	END

	INSERT into sysuser (UserId,UserName,Passwords) VALUES
	(@StaffId,@UserName,@PassWord);

	PRINT N'Tạo thành công'
	RETURN 0;
END;
GO
--Proc tạo 1 customer mới 
CREATE PROC TaoCustomer @Email NVARCHAR(50),@Ten NVARCHAR(50), @Id char(10),@DiaChi NVARCHAR(100),@SDT char(11),@PassWord CHAR(100)
AS
BEGIN
	set NOCOUNT ON;
	IF exists (SELECT 1 from CustomerDetail where Email = @Email or SDT = @SDT)
	BEGIN
	 	PRINT N'Email hoặc SDT đã được sử dụng'
		RETURN -1;
	END

	INSERT into Customer(CustomerId,UserName,Passwords) VALUES
	(@Id,@Email,@PassWord);
	INSERT into CustomerDetail(CustomerId,TenKhach,Email,DiaChi,SDT) VALUES
	(@Id,@Ten,@Email,@DiaChi,@SDT);
	PRINT N'Đã thêm thành công'
	RETURN 0;
END
GO
--proc cập nhật lại trạng thãi của đơn hàng (Các đơn có trạng thái Hoàn thành và đã hủy không được cập nhật)
CREATE PROC CapNhatTrangThaiDon @MaDon char(10), @TrangThai NVARCHAR(50)
AS
BEGIN
	set NOCOUNT ON;
	DECLARE @OldStatus nvarchar(50);
	set @OldStatus = (select TrangThai from donhang WHERE MaDon = @MaDon)
	IF @OldStatus is null 
	BEGIN
		PRINT N'đơn hàng không tồn tại'
		RETURN -1;
	END

	IF @OldStatus IN (N'Hoàn thành',N'Đã hủy')
	BEGIN
		PRINT N'Đơn hàng đã hoàn thành hoặc đã bị hủy không thể cập nhật'
		RETURN -1;
	END

	UPDATE donhang SET TrangThai = @TrangThai WHERE MaDon = @MaDon
END
go

--Function của database
--Func trả về doanh thu của 1 cửa hàng trong khoảng thời gian 
create FUNCTION DoanhThuCuaHang (@From date, @To date, @MaCH char(10))
RETURNS money
as
begin 
	Declare @DoanhThu money = (select iSnull(sum(chi_tiet_don_hang.SoLuong * sanpham.DonGia),0) as doanhthu
					from donhang
					join chi_tiet_don_hang on chi_tiet_don_hang.MaDon = donhang.MaDon
					join sanpham on chi_tiet_don_hang.MaSP = sanpham.MaSP
					join sysuser on donhang.UserId = sysuser.UserId
					join staff on staff.StaffId = sysuser.UserId
					join cuahang on cuahang.CuaHangId = staff.CuaHangId
					where cuahang.CuaHangId = @MaCH and (cast(donhang.NgayHoangThanh as date) between @From and @To)
					);
	RETURN @DoanhThu;
end;
go
--Func tính thành tiền cho 1 đơn hàng với công thức thành tiền bằng sum (Số lượng * đơn giá ) * 1.1
CREATE FUNCTION ThanhTienDonHang (@MaDon char(10))
RETURNS money
AS
BEGIN
	
	IF not exists (select 1 from donhang WHERE MaDon = @MaDon)
	BEGIN
		RETURN 0; 
	END

	DECLARE @ThanhTien money = (Select ISNULL(SUM(sanpham.DonGia * chi_tiet_don_hang.SoLuong)* 1.1 ,0) as ThanhTien
								from chi_tiet_don_hang
								join sanpham on chi_tiet_don_hang.MaSP = sanpham.MaSP
								WHERE chi_tiet_don_hang.MaDon = @MaDon)

	RETURN @ThanhTien;
END
GO

--Func lấy tất cả sản phẩm theo danh muc 
CREATE FUNCTION SanPhamTheoDm (@MaDM char(10))
RETURNS TABLE
AS
	RETURN  
	(
		SELECT * 
		FROM sanpham 
		WHERE sanpham.maDM = @MaDM 
	);
GO

--Func trả về thời gian xử lý của 1 đơn hàng nếu đơn hàng chưa hoàn thành hoặc bị hủy sẽ trả về 0
CREATE FUNCTION ThoiGianXuLyDon (@maDon char(10))
RETURNS INT
AS
BEGIN
	DECLARE @TGXuLy int;
	select @TGXuLy = case 
						when dh.TrangThai = N'Hoàn thành' and dh.NgayHoangThanh is not null
						then DATEDIFF(MINUTE, dh.NgayNhan,dh.NgayHoangThanh)
						else 0
					end
					from donhang dh 
					WHERE dh.MaDon = @maDon
	RETURN @TGXuLy;
END
go

--Func nhận vào ngày và cửa hàng id trả về tổng số đơn trong ngày hôm đó 
CREATE FUNCTION TongDonTheoNgayCuaCH (@date date, @MaCH CHAR(10),@TrangThai NVARCHAR(50))
RETURNS INT
as
BEGIN
	if not exists (SELECT 1 from cuahang WHERE CuaHangId = @MaCH)
	BEGIN
		RETURN 0;
	END
	DECLARE @TongSoDon int;
	SELECT @TongSoDon = COUNT(donhang.MaDon)
						from donhang
						JOIN sysuser on donhang.UserId = sysuser.UserId
						JOIN staff on sysuser.UserId = staff.StaffId
						JOIN cuahang on staff.CuaHangId = cuahang.CuaHangId
						WHERE cuahang.CuaHangId = @MaCH  and donhang.TrangThai = @TrangThai
						GROUP BY donhang.MaDon
	RETURN @TongSoDon;
END
go

--Kiểm tra tính hợp lệ của 1 Email 
CREATE FUNCTION KiemTraHopLeEmail (@Email nvarchar(50))
RETURNS BIT
AS
BEGIN
	IF @Email IS NULL OR @Email = ''
	BEGIN
		RETURN 0 
	END
	if @Email LIKE N'%@%.%'
	BEGIN
		RETURN 1
	END
	RETURN 0
END
go

--Lấy ra sản phẩm có nhiều lượt mua nhất trong khoảng thời gian
CREATE FUNCTION SanPhamBanChayNhat (@From date, @To date)
RETURNS TABLE
AS
	RETURN
	(
		SELECT sanpham.MaSP, sanpham.TenSP,sanpham.DonGia
		From sanpham
		JOIN chi_tiet_don_hang ON chi_tiet_don_hang.MaSP = sanpham.MaSP
		JOIN donhang ON donhang.MaDon = chi_tiet_don_hang.MaDon
		WHERE CAST( donhang.NgayHoangThanh as date) BETWEEN @From and @To
	);
go

---Cursor + Proc/Trigger của hệ thống
--Proc gửi Email cho các khách hàng có đơn hoàn thành (Chỉ in ra thông báo đã gửi mail khônng cần gửi thật)
CREATE PROC GuiMailChoKhach 
AS
BEGIN
	set NOCOUNT ON;
	DECLARE Cur_KH CURSOR 
	FOR SELECT MaDon CustomerId, TrangThai FROM donhang

	DECLARE @Customer CHAR(10), @TrangThai NVARCHAR(50), @maDon char(10);

	OPEN Cur_KH
	FETCH NEXT FROM Cur_KH into @maDon,  @Customer, @TrangThai;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @TrangThai = N'Hoàn thành'
		BEGIN
			PRINT N'đơn hàng: '+ @maDon + N'Đã hoàn thành' + N' ->  mail đến khách hàng: '+ @Customer
		END 
		FETCH NEXT FROM Cur_KH into @maDon,  @Customer, @TrangThai;
	END

	close Cur_KH
	DEALLOCATE Cur_KH
END
GO
--Cập nhật thưởng cho nhân viên = 2% tổng doanh thu các đơn hàng đã hoàn thành, nếu không có thì cập nhật là 0
CREATE PROC CapNhatThuongNhanVien 
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @ThangHienTai DATE = DATEADD(month, DATEDIFF(month, 0, SYSDATETIME()), 0);
	
	DECLARE Cur_nv CURSOR FOR SELECT UserId from sysuser

	Declare @MaNV CHAR(10)

	OPEN Cur_nv
	FETCH NEXT FROM Cur_nv INTO @MaNV

	WHILE @@FETCH_STATUS = 0
	BEGIN
		Declare @DoanhThu money;
		SELECT @DoanhThu = ISNULL(
            (
                SELECT 
                    SUM(sp.DonGia * ctdh.SoLuong * 1.1) -- 1.1 giả định là VAT/phí
                FROM 
                    donhang dh
                JOIN 
                    chi_tiet_don_hang ctdh ON ctdh.MaDon = dh.MaDon
                JOIN 
                    sanpham sp ON sp.MaSP = ctdh.MaSP
                WHERE 
                    -- So sánh NgayHoanThanh với tháng hiện tại
                    dh.NgayHoangThanh >= @ThangHienTai 
                    AND dh.NgayHoangThanh < DATEADD(month, 1, @ThangHienTai) -- Tới ngày đầu tháng sau
                    AND dh.TrangThai = N'Hoàn thành' 
                    AND dh.UserId = @MaNV
            )
            , 0 -- Trả về 0 nếu truy vấn không tìm thấy đơn hàng Hoàn thành
        );
		UPDATE staff SET Thuong = @DoanhThu * 0.02 WHERE StaffId = @MaNV
		FETCH NEXT FROM Cur_nv INTO @MaNV
		
	END
	CLOSE Cur_nv
	DEALLOCATE Cur_nv
END
GO

--Cập nhật trạng thái cảnh báo cho sản phẩm không có doanh thu trong 6 tháng qua
CREATE PROC CapNhatTinhTrangCanhBaoSP
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @NgayMoc DATETIME2 = DATEADD(month, -6, SYSDATETIME());

	DECLARE Cur_sp CURSOR FOR SELECT MaSP from sanpham
	DECLARE @maSP char(10)
	OPEN Cur_sp

	FETCH NEXT FROM Cur_sp into @maSP

	WHILE @@FETCH_STATUS = 0
	BEGIN
		Declare @soLuongDon int;

		
		SELECT @soLuongDon = COUNT(MaSP) FROM chi_tiet_don_hang
		JOIN donhang ON donhang.MaDon = chi_tiet_don_hang.MaDon
		WHERE chi_tiet_don_hang.MaDon = @maSP
		AND donhang.NgayNhan >= @NgayMoc
		
		if @soLuongDon = 0 
			BEGIN
				UPDATE sanpham SET [status] = N'Không có doanh thu' where sanpham.MaSP = @maSP
			END
		
		FETCH NEXT FROM Cur_sp into @maSP
	END

	CLOSE Cur_sp
	DEALLOCATE Cur_sp
END
GO

--Cursor + proc cập nhật trạng thái không hoạt động cho các khách không mua hàng trong 6 tháng 
CREATE PROC CapNhatNgungHoatDong
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @NgayMoc DATETIME2 = DATEADD(month, -12, SYSDATETIME());

	DECLARE cur_kh CURSOR FOR
	SELECT CustomerId From Customer

	declare @makh char(10)
	OPEN cur_kh
	FETCH NEXT FROM cur_kh INTO @makh

	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @SoLuongDon INT;

		SELECT @SoLuongDon =  COUNT(donhang.MaDon)
		from donhang 
		WHERE donhang.CustomerId = @makh AND donhang.NgayNhan > @NgayMoc

		IF @SoLuongDon = 0
		BEGIN
			UPDATE Customer SET statusC = N'Ngưng hoạt động' where CustomerId = @makh
		END
		FETCH NEXT FROM cur_kh INTO @makh
	END

	Close cur_kh
	DEALLOCATE cur_kh
END
GO

--Proc KiemTraVaCapNhatTrangThaiCuaHang
CREATE PROC KiemTraVaCapNhatTrangThaiCuaHang 
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @NgayMoc DATETIME2 = DATEADD(month, -12, SYSDATETIME());
	DECLARE @NgayHienTai datetime2 = SYSDATETIME();

	DECLARE Cur_CH CURSOR FOR  SELECT CuaHangId from cuahang where statusS != N'Ngưng hoạt động';
	DECLARE @maCH char(10);
	OPEN Cur_CH;
	FETCH NEXT FROM Cur_CH INTO @maCH;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @DoanhThu money;
		Declare @NewStatus NVARCHAR(50);
		Set @DoanhThu = dbo.DoanhThuCuaHang(@NgayMoc,@NgayHienTai,@maCH)
		if @DoanhThu < 100000000
		BEGIN
			SET @NewStatus = N'Doanh số thấp'
		END
		ELSE IF @DoanhThu < 400000000
		BEGIN
			set @NewStatus = N'Doanh số bình thường'
		END
		ELSE 
		BEGIN
			set @NewStatus = N'Doanh số cao'
		END

		update cuahang SET statusS = @NewStatus WHERE CuaHangId = @maCH;
		FETCH NEXT FROM Cur_CH INTO @maCH;
	END

	CLOSE Cur_CH;
	DEALLOCATE Cur_CH;

END
go