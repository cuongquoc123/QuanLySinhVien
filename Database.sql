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
	StaffId char(11) primary key,
	CCCD char(11) not null unique,
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
	status nvarchar(30),
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
	tenDM nvarchar(50) not null,
	LoaiDM nvarchar(50) not null
);
go

drop table if exists sanpham;
create table sanpham (
	MaSP char(20) primary key,
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
	MaSP char(20) not null, 
	primary key (MaDon,MaSP),
	Constraint FK_CTDH_SP foreign key (MaSP) references sanpham(MaSP),
	Constraint FK_CTDH_DH foreign key (MaDon) references donhang(MaDon)
);
go

INSERT INTO danhmuc (maDM, tenDM, LoaiDM) VALUES
-- Loại 'Đồ uống'
('DM_CF', N'Cà phê', N'Đồ uống'),
('DM_TS', N'Trà Sữa', N'Đồ uống'),
('DM_TT', N'Trà Trái Cây', N'Đồ uống'),

-- Loại 'Đồ ăn'
('DM_AK', N'Món Ăn Kèm', N'Đồ ăn'),
('DM_BN', N'Bánh Ngọt', N'Đồ ăn'),
('DM_AV', N'Đồ Ăn Vặt', N'Đồ ăn'),

-- Loại 'Sản phẩm khác'
('DM_SPK01', N'Cà Phê Hạt & Bột', N'Sản phẩm khác'),
('DM_SPK02', N'Ly & Cốc Thương Hiệu', N'Sản phẩm khác'),
('DM_SPK03', N'Dụng Cụ Pha Chế', N'Sản phẩm khác');

INSERT INTO sanpham (MaSP, TenSP, DonGia, Anh, status, Mota, maDM) VALUES
-- Đồ uống > Cà phê (DM_CF)
('SP_CF01', N'Cà Phê Đen Đá', 20000, 'cf_denda.jpg', N'Có sẵn', N'Cà phê Robusta nguyên chất, rang xay đậm đà.', 'DM_CF'),
('SP_CF02', N'Cà Phê Sữa Đá', 25000, 'cf_suada.jpg', N'Có sẵn', N'Hương vị cà phê đậm đà hòa quyện cùng sữa đặc ngọt ngào.', 'DM_CF'),
('SP_CF03', N'Bạc Xỉu', 28000, 'bacxiu.jpg', N'Có sẵn', N'Nhiều sữa hơn cà phê, phù hợp cho người thích vị ngọt béo.', 'DM_CF'),
('SP_CF04', N'Espresso', 30000, 'espresso.jpg', N'Có sẵn', N'Một shot cà phê đậm đặc được pha bằng máy.', 'DM_CF'),

-- Đồ uống > Trà Sữa (DM_TS)
('SP_TS01', N'Trà Sữa Trân Châu Đường Đen', 45000, 'ts_duongden.jpg', N'Có sẵn', N'Trà sữa béo ngậy với trân châu đường đen dai mềm, thơm nức.', 'DM_TS'),
('SP_TS02', N'Trà Sữa Truyền Thống', 35000, 'ts_truyenthong.jpg', N'Có sẵn', N'Hương vị trà sữa nguyên bản, quen thuộc.', 'DM_TS'),
('SP_TS03', N'Trà Sữa Matcha', 42000, 'ts_matcha.jpg', N'Có sẵn', N'Vị trà xanh matcha Nhật Bản thơm lừng, hơi chát nhẹ.', 'DM_TS'),
('SP_TS04', N'Trà Sữa Oolong Kem Phô Mai', 48000, 'ts_oolongcheese.jpg', N'Có sẵn', N'Trà Oolong thanh mát phủ lớp kem phô mai mặn béo.', 'DM_TS'),

-- Đồ uống > Trà Trái Cây (DM_TT)
('SP_TT01', N'Trà Đào Cam Sả', 42000, 'tra_daocamsa.jpg', N'Có sẵn', N'Thức uống giải nhiệt hoàn hảo từ đào, cam tươi và sả.', 'DM_TT'),
('SP_TT02', N'Trà Vải', 40000, 'tra_vai.jpg', N'Có sẵn', N'Trà olong thanh mát kết hợp với những quả vải căng mọng.', 'DM_TT'),
('SP_TT03', N'Trà Chanh Mật Ong', 35000, 'tra_chanhmatong.jpg', N'Có sẵn', N'Vị chua thanh của chanh và ngọt dịu của mật ong.', 'DM_TT'),

-- Đồ ăn > Món Ăn Kèm (DM_AK)
('SP_AK01', N'Hướng Dương', 15000, 'huongduong.jpg', N'Có sẵn', N'Đĩa hướng dương rang bơ thơm lừng.', 'DM_AK'),
('SP_AK02', N'Khô Gà Lá Chanh', 30000, 'khoga.jpg', N'Có sẵn', N'Thịt gà xé sợi sấy khô cùng lá chanh, vị cay mặn ngọt.', 'DM_AK'),
('SP_AK03', N'Khoai Tây Chiên', 25000, 'khoaitaychien.jpg', N'Có sẵn', N'Khoai tây giòn rụm ăn kèm tương cà hoặc tương ớt.', 'DM_AK'),

-- Đồ ăn > Bánh Ngọt (DM_BN)
('SP_BN01', N'Tiramisu', 35000, 'tiramisu.jpg', N'Có sẵn', N'Bánh ngọt tráng miệng vị cà phê của Ý.', 'DM_BN'),
('SP_BN02', N'Bánh Phô Mai Chanh Dây', 40000, 'cheesecake_passion.jpg', N'Có sẵn', N'Bánh phô mai béo ngậy kết hợp vị chua thơm của chanh dây.', 'DM_BN'),
('SP_BN03', N'Croissant Bơ Tỏi', 30000, 'croissant_garlic.jpg', N'Có sẵn', N'Bánh sừng bò nướng giòn với sốt bơ tỏi thơm lừng.', 'DM_BN'),

-- Đồ ăn > Đồ Ăn Vặt (DM_AV)
('SP_AV01', N'Bắp Rang Bơ Caramel', 30000, 'baprangbo.jpg', N'Có sẵn', N'Bắp rang giòn tan phủ lớp caramel ngọt ngào.', 'DM_AV'),
('SP_AV02', N'Phô Mai Que', 35000, 'phomaique.jpg', N'Có sẵn', N'Phô mai mozzarella chiên xù, kéo sợi hấp dẫn.', 'DM_AV'),
('SP_AV03', N'Cá Viên Chiên', 25000, 'cavienchien.jpg', N'Có sẵn', N'Món ăn vặt quen thuộc, chấm cùng tương ớt.', 'DM_AV'),

-- Sản phẩm khác > Cà Phê Hạt & Bột (DM_SPK01)
('SP_SPK01_01', N'Bột Cà Phê Robusta (250g)', 120000, 'bot_robusta.jpg', N'Còn hàng', N'Bột cà phê Robusta rang mộc, nguyên chất.', 'DM_SPK01'),
('SP_SPK01_02', N'Hạt Cà Phê Arabica (250g)', 150000, 'hat_arabica.jpg', N'Còn hàng', N'Hạt cà phê Arabica Cầu Đất, hương thơm quyến rũ.', 'DM_SPK01'),
('SP_SPK01_03', N'Phin Nhôm Pha Cà Phê', 50000, 'phin_nhom.jpg', N'Còn hàng', N'Phin nhôm loại vừa, dùng để pha cà phê phin truyền thống.', 'DM_SPK01'),

-- Sản phẩm khác > Ly & Cốc Thương Hiệu (DM_SPK02)
('SP_SPK02_01', N'Ly Sứ Logo Quán', 90000, 'ly_su_logo.jpg', N'Còn hàng', N'Ly sứ cao cấp in logo, dung tích 350ml.', 'DM_SPK02'),
('SP_SPK02_02', N'Cốc Giữ Nhiệt', 250000, 'coc_giunhiet.jpg', N'Còn hàng', N'Cốc inox giữ nhiệt nóng lạnh lên đến 8 tiếng.', 'DM_SPK02'),
('SP_SPK02_03', N'Bình Nước Thủy Tinh', 120000, 'binh_thuytinh.jpg', N'Còn hàng', N'Bình nước thủy tinh có quai xách và bọc vải.', 'DM_SPK02'),

-- Sản phẩm khác > Dụng Cụ Pha Chế (DM_SPK03)
('SP_SPK03_01', N'Bộ Dụng Cụ V60', 450000, 'v60_kit.jpg', N'Còn hàng', N'Bộ pha cà phê pour-over V60 cho người mới bắt đầu.', 'DM_SPK03'),
('SP_SPK03_02', N'Bình French Press', 350000, 'french_press.jpg', N'Còn hàng', N'Dụng cụ pha cà phê kiểu Pháp, dung tích 600ml.', 'DM_SPK03'),
('SP_SPK03_03', N'Ấm Cổ Ngỗng', 550000, 'gooseneck_kettle.jpg', N'Tạm hết hàng', N'Ấm chuyên dụng để pha cà phê thủ công, kiểm soát dòng nước.', 'DM_SPK03');


insert into sysrole (RoleId, RoleName) values 
('R01', N'admin'),
('R02', N'manager'),
('R03', N'cashier');

INSERT INTO cuahang (CuaHangId, TenCH, DiaChi, SDT, statuss, Email) VALUES
('CH001', N'Cửa hàng Trung tâm', N'123 Nguyễn Huệ, Quận 1, TPHCM', '0908111222', N'Đang hoạt động', 'trungtam.q1@email.com'),
('CH002', N'Cửa hàng Sân bay', N'456 Trường Sơn, Quận Tân Bình, TPHCM', '0912333444', N'Đang hoạt động', 'sanbay.tphcm@email.com'),
('CH003', N'Cửa hàng Thảo Điền', N'789 Võ Nguyên Giáp, Quận 2, TPHCM', '0987555666', N'Đang hoạt động', 'thaodien.q2@email.com'),
('CH004', N'Cửa hàng Quận 7', N'101 Tôn Dật Tiên, Quận 7, TPHCM', '0933777888', N'Tạm ngưng sửa chữa', 'quan7.tdt@email.com'),
('CH005', N'Cửa hàng Gò Vấp', N'222 Quang Trung, Quận Gò Vấp, TPHCM', '0945999000', N'Sắp khai trương', 'govap.qt@email.com');
delete from sysuser
select * from sysrole
select * from cuahang
