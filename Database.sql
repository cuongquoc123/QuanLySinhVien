--Tạo CSDL 
use master;
Drop Database if exists ql_cua_hang;
create Database ql_cua_hang;
go

use ql_cua_hang;

drop table if exists cuahang;
create table cuahang(
	CuaHangId char(10) primary key,
	TenCH nvarchar(50) not null,
	DiaChi nvarchar(50) not null,
	SDT char(11),
	statuss nvarchar(100),
	Email varchar(100)
);
go

drop table if exists staff;
create table staff(
	CCCD char(11) primary key,
	Ten nvarchar(50) not null,
	DiaChi nvarchar(50) not null,
	NgaySinh date,
	Vtri nvarchar(50),
	Luong Decimal(18,3),
	Avatar nvarchar(500) not null,
	StatuSs nvarchar(100),
	CuaHangId char(10),
	Constraint FK_CH_STAFF foreign key (CuaHangId) references cuahang (CuaHangId) 
)
go

drop table if exists phieu_NL; 
create table phieu_NL(
	MaPhieu char(10) primary key,
	ngay_yeu_cau date not null,
	CuaHangId char(10),
	Constraint FK_CH_PNL foreign key (CuaHangId) references cuahang (CuaHangId) 
);
go

drop table if exists nguyenlieu;
create table nguyenlieu (
	MaNguyenLieu char(10) primary key,
	TenNguyenLieu nvarchar(50) not null,
	DVT nvarchar(20) not null
);
go

drop table if exists chi_tiet_yeu_cau;
create table chi_tiet_yeu_cau (
	MaNguyenLieu char(10) not null, 
	SoLuong int Check(SoLuong > 0),
	MaPhieu char(10) not null,
	Primary key (MaNguyenLieu,MaPhieu),
	Constraint FK_CTYC_PHIEU foreign key (MaPhieu) references phieu_NL(MaPhieu),
	Constraint FK_CTYC_NL foreign key (MaNguyenLieu) references nguyenlieu(MaNguyenLieu)
);
go

drop table if exists kho;
create table kho (
	MaKho char(10) primary key,
	SoLuongTon int check (SoLuongTon >= 0),
	MaNguyenLieu char(10),
	Constraint FK_Kho_NguyenLieu foreign key (MaNguyenLieu) references nguyenlieu(MaNguyenLieu),
	CuaHangId char(10),
	Constraint FK_Kho_CH foreign key (CuaHangId) references cuahang(CuaHangId)
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
	NgaySinh date,
	DiaChi nvarchar(50),
	UserName nvarchar(100) unique not null,
	Avatar nvarchar(500),
	Passwords nvarchar(100) not null,
	CuaHangId char(10) not null,
	Constraint FK_CH_User foreign key (CuaHangId) references cuahang(CuaHangId),
	RoleId char(10)
	Constraint FK_USER_ROLE foreign key (RoleId) references sysrole(RoleId)
);
go

drop table if exists danhmuc;
create table danhmuc (
	maDM char(10) primary key,
	tenDM nvarchar(50) not null
);
go

drop table if exists sanpham;
create table sanpham (
	MaSP char(10) primary key,
	DonGia decimal(18,3) not null,
	TenSP nvarchar(50) not null,
	Anh nvarchar(500),
	status nvarchar(50),
	Mota text,
	maDM char(10) not null,
	Constraint FK_SP_DM foreign key (maDM) references danhmuc(maDM)
);
go

drop table if exists donhang;
create table donhang(
	MaDon char(10) primary key,
	ThanhTien decimal(18,3) not null,
	TrangThai nvarchar(50) check (TrangThai in (N'Hoang Thanh', N'Da Huy', N'Tiep Nhan', N'Dang Xu Ly')),
	NgayNhan Date not null,
	NgayHoangThanh date,
	CuaHangId char(10) ,
	Constraint FK_CH_DH foreign key (CuaHangId) references cuahang(CuaHangId),
	UserId char(10) , 
	Constraint FK_NV_DH foreign key (UserId) references sysuser(UserId)
); 
go

drop table if exists chi_tiet_don_hang;
create table chi_tiet_don_hang (
	SoLuong int not null check (SoLuong > 0),
	MaDon char(10) not null,
	MaSP char(10) not null, 
	primary key (MaDon,MaSP),
	Constraint FK_CTDH_SP foreign key (MaSP) references sanpham(MaSP),
	Constraint FK_CTDH_DH foreign key (MaDon) references donhang(MaDon)
);
go

-- Sửa lại bảng danhmuc với tiền tố N'
INSERT INTO danhmuc (maDM, tenDM) VALUES
('DM_CF', N'Cà phê'),
('DM_TS', N'Trà Sữa'),
('DM_TT', N'Trà Trái Cây'),
('DM_DX', N'Đá Xay'),
('DM_NE', N'Nước Ép & Sinh Tố'),
('DM_AK', N'Món Ăn Kèm');

-- Sửa lại bảng sanpham với tiền tố N'
INSERT INTO sanpham (MaSP, TenSP, DonGia, Anh, status, Mota, maDM) VALUES
-- Các món cà phê (DM_CF)
('SP_CF01', N'Cà Phê Đen Đá', 20.00, 'cf_denda.jpg', N'Có sẵn', N'Cà phê Robusta nguyên chất, rang xay đậm đà.', 'DM_CF'),
('SP_CF02', N'Cà Phê Sữa Đá', 25.00, 'cf_suada.jpg', N'Có sẵn', N'Hương vị cà phê đậm đà hòa quyện cùng sữa đặc ngọt ngào.', 'DM_CF'),
('SP_CF03', N'Bạc Xỉu', 28.00, 'bacxiu.jpg', N'Có sẵn', N'Nhiều sữa hơn cà phê, phù hợp cho người thích vị ngọt béo.', 'DM_CF'),

-- Các món trà sữa (DM_TS)
('SP_TS01', N'Trà Sữa Trân Châu Đường Đen', 45.00, 'ts_duongden.jpg', N'Có sẵn', N'Trà sữa béo ngậy với trân châu đường đen dai mềm, thơm nức.', 'DM_TS'),
('SP_TS02', N'Trà Sữa Truyền Thống', 35.00, 'ts_truyenthong.jpg', N'Có sẵn', N'Hương vị trà sữa nguyên bản, quen thuộc.', 'DM_TS'),

-- Các món trà trái cây (DM_TT)
('SP_TT01', N'Trà Đào Cam Sả', 42.00, 'tra_daocamsa.jpg', N'Có sẵn', N'Thức uống giải nhiệt hoàn hảo từ đào, cam tươi và sả.', 'DM_TT'),
('SP_TT02', N'Trà Vải', 40.00, 'tra_vai.jpg', N'Hết hàng tạm thời', N'Trà olong thanh mát kết hợp với những quả vải căng mọng.', 'DM_TT'),

-- Các món đá xay (DM_DX)
('SP_DX01', N'Cà Phê Cốt Dừa Đá Xay', 50.00, 'cf_cotdua_daxay.jpg', N'Có sẵn', N'Sự kết hợp độc đáo giữa cà phê, cốt dừa và đá xay mịn.', 'DM_DX'),
('SP_DX02', N'Matcha Đá Xay', 55.00, 'matcha_daxay.jpg', N'Có sẵn', N'Bột matcha Nhật Bản cao cấp xay mịn với đá và kem tươi.', 'DM_DX'),

-- Nước ép (DM_NE)
('SP_NE01', N'Nước Ép Cam', 35.00, 'nuocep_cam.jpg', N'Có sẵn', N'100% cam tươi vắt, không thêm đường.', 'DM_NE'),

-- Món ăn kèm (DM_AK)
('SP_AK01', N'Hướng Dương', 15.00, 'huongduong.jpg', N'Có sẵn', N'Đĩa hướng dương rang bơ thơm lừng.', 'DM_AK');
select * from donhang;