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
	UserName nvarchar(100) unique not null,
	Passwords nvarchar(100) not null,
);
go
drop table if exists CustomerDetail;
create table CustomerDetail(
	CustomerId char(10) primary key,
	TenKhach nvarchar(50),
	CCCD char(11),
	SDT char(10),
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
create trigger UpdateSoLuongTonKho on chi_tiet_yeu_cau
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
--Proc báo cáo doanh thu của cửa hàng từ Date From đến Date To
--Chuyển cái này function sau khi làm xong proc 
create proc DoanhThuCuaHang @From date, @To date, @MaCH char(10), @DoanhThu money output
as
begin 
	set nocount on;
	set @DoanhThu = (select iSnull(sum(chi_tiet_don_hang.SoLuong * sanpham.DonGia),0) as doanhthu
					from donhang
					join chi_tiet_don_hang on chi_tiet_don_hang.MaDon = donhang.MaDon
					join sanpham on chi_tiet_don_hang.MaSP = sanpham.MaSP
					join sysuser on donhang.UserId = sysuser.UserId
					join staff on staff.StaffId = sysuser.UserId
					join cuahang on cuahang.CuaHangId = staff.CuaHangId
					where cuahang.CuaHangId = @MaCH and (cast(donhang.NgayHoangThanh as date) between @From and @To)
					);
end;
go

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
--proc 