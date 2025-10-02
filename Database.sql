use master
DROP DATABASE QL_DiemDanhSV;
GO

-- Tạo database
CREATE DATABASE QL_DiemDanhSV;
GO

USE QL_DiemDanhSV;
GO

-- Bảng Khoa
CREATE TABLE Khoa (
    MaKhoa CHAR(10) PRIMARY KEY,
    TenKhoa NVARCHAR(100) NOT NULL
);
-- Bảng Khoa
CREATE TABLE Nganh (
    MaNganh CHAR(10) PRIMARY KEY,
    TenNganh NVARCHAR(100) NOT NULL
);

-- Bảng Lớp Hành Chính
CREATE TABLE LopHanhChinh (
    MaLopHC CHAR(10) PRIMARY KEY,
    TenLopHC NVARCHAR(100) NOT NULL,
    SiSo INT,
    MaNganh CHAR(10) NOT NULL,
    FOREIGN KEY (MaNganh) REFERENCES Nganh(MaNganh)
);
-- Bảng Sinh Viên
CREATE TABLE SinhVien (
    MSSV CHAR(15) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    SDT NVARCHAR(20),
    Email NVARCHAR(100),
    DiaChi NVARCHAR(200),
    MaLopHC CHAR(10) NOT NULL,
    FOREIGN KEY (MaLopHC) REFERENCES LopHanhChinh(MaLopHC)
);
-- Bảng Giảng Viên
CREATE TABLE GiangVien (
    MaGV CHAR(10) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    SDT NVARCHAR(20),
    MaKhoa CHAR(10) NOT NULL,
    FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa)
);
-- Bảng Môn Học
CREATE TABLE MonHoc (
    MaMon CHAR(10) PRIMARY KEY,
    TenMon NVARCHAR(100) NOT NULL,
    SoTinChi INT,
    SoTiet INT NOT NULL,
    MaNganh CHAR(10) NOT NULL,
    FOREIGN KEY (MaNganh) REFERENCES Nganh(MaNganh)
);

-- Bảng Lớp Học Phần
CREATE TABLE LopHocPhan (
    MaLopHP CHAR(10) PRIMARY KEY,
    TenLopHP NVARCHAR(100) NOT NULL,
    MaMon CHAR(10) NOT NULL,
    MaGV CHAR(10) NOT NULL,
    HocKy NVARCHAR(10),
    NamHoc NVARCHAR(20),
    constraint FK_MH_LHP FOREIGN KEY (MaMon) REFERENCES MonHoc(MaMon),
    constraint FK_GV_LHP FOREIGN KEY (MaGV) REFERENCES GiangVien(MaGV)
);
-- Bảng Đăng Ký Lớp Học Phần
CREATE TABLE DangKyLopHP (
    MSSV CHAR(15),
    MaLopHP CHAR(10),
    PRIMARY KEY (MSSV, MaLopHP),
    FOREIGN KEY (MSSV) REFERENCES SinhVien(MSSV),
    FOREIGN KEY (MaLopHP) REFERENCES LopHocPhan(MaLopHP)
);

-- Bảng Điểm Danh
CREATE TABLE DiemDanh (
    MaDiemDanh INT IDENTITY(1,1) PRIMARY KEY,
    MSSV CHAR(15) NOT NULL,
    MaLopHP CHAR(10) NOT NULL,
    NgayHoc DATE NOT NULL,
    TrangThai NVARCHAR(20) CHECK (TrangThai IN ('Có mặt', 'Không phép', 'Có phép')),
    FOREIGN KEY (MSSV) REFERENCES SinhVien(MSSV),
    FOREIGN KEY (MaLopHP) REFERENCES LopHocPhan(MaLopHP)
);
CREATE TABLE LichHoc (
    MaLichHoc INT IDENTITY(1,1) PRIMARY KEY,
    MaLopHP CHAR(10) NOT NULL,     
    NgayHoc DATE NOT NULL,
    TietBatDau INT NOT NULL,
    SoTiet INT NOT NULL,
    PhongHoc NVARCHAR(50),
    GhiChu NVARCHAR(200),
    FOREIGN KEY (MaLopHP) REFERENCES LopHocPhan(MaLopHP)
);

ALTER TABLE LichHoc 
add DayOfWeek NVARCHAR(30) NOt NULL

SELECT * FROM LichHoc
DELETE from LichHoc


ALTER TABLE DIEMDANH
ADD MaLichHoc INT NULL,
    FOREIGN KEY (MaLichHoc) REFERENCES LichHoc(MaLichHoc);

use QL_DiemDanhSV
CREATE TABLE roles (
    roleId char(3) PRIMARY KEY,
    roleName NVARCHAR(30)
);

CREATE TABLE users (
    username char(10) PRIMARY KEY,
    roleId char (3),
    passwords Nvarchar(50),
);

ALTER TABLE users 
ADD CONSTRAINT FK_USER_ROLE FOREIGN KEY (roleId) REFERENCES roles(roleId)
USE QL_DiemDanhSV
ALTER TABLE SinhVien 
add  Avatar VARCHAR(255)