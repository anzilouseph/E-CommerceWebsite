
create database ECommerce;
use ECommerce;

create table Users(UserId char(36) primary key default(uuid()),FullName varchar(100) not null , Phone varchar(50) unique not null, Email varchar(50) unique not null,Password varchar(255) not null,Role enum ('Admin','User') default 'User', Salt varchar(255));

create table Products(ProductId char(36) primary key default(UUID()),ProductName varchar(100) not null,Description Text ,Price decimal(10,2) NOT NULL ,StockQuantity int NOT NULL,ImageUrl varchar(500) null,CategoryId char(36) ,foreign key (CategoryId) references Category(CategoryId));

create table Orders(OrderId char(36) primary key default(UUID()),UserId char(36) null,ProductId char(36) NULL,Quantity int not null,TotalAmount decimal(10,2) not null,OrderDate date DEFAULT (CURDATE()),Status enum('Complete','Pending') default 'Pending' , foreign key (UserId) references  Users(UserId) on delete set null, foreign key (ProductId) references Products(ProductId) on delete set null) ;

CREATE TABLE Cart (
    CartId CHAR(36) PRIMARY KEY DEFAULT (UUID()),  -- Unique Cart Entry Identifier
    UserId CHAR(36) NULL,  -- User who added the item
    ProductId CHAR(36) NULL,  -- Product added to cart
    Quantity INT NOT NULL DEFAULT 1,  -- Number of units
    TotalPrice DECIMAL(10,2) NOT NULL,  -- Price * Quantity
	AddedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,  -- When added to cart
    UNIQUE (UserId, ProductId),  -- Ensures user cannot add the same product multiple times
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE SET NULL,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE SET NULL
);



CREATE TABLE Wishlist (WishlistId CHAR(36) PRIMARY KEY DEFAULT(UUID()),UserId CHAR(36) NOT NULL,ProductId CHAR(36) NOT NULL,AddedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE CASCADE);

create Table Category(CategoryId char(36) primary key default(UUID()),CategoryName VARCHAR(255) NOT NULL,Description Varchar(255));


alter table Products Modify ImageUrl varchar(500) unique;  -- ---table changing code---
alter table Products Modify ProductName varchar(255) not null unique;  -- ---table changing code---
alter table Users Add column ProfileImage varchar(500) unique;  -- ---table changing code---
ALTER TABLE Wishlist ADD UNIQUE (UserId, ProductId);  -- ---table changing code---

drop table cart;

show TABLES;
describe Products;
select * from Users;

select * from Products;


INSERT INTO Category (CategoryName, Description) VALUES
('Football', 'Official match and training footballs'),
('Football Boots', 'Studded and turf football boots'),
('Football Jerseys', 'Club and national team jerseys'),
('Shin Guards', 'Protective shin guards for players'),
('Goalkeeper Gloves', 'Gloves for goalkeepers with grip'),
('Football Socks', 'Long socks for football players'),
('Training Cones', 'Cones used for football drills'),
('Football Nets', 'Goal nets for training and matches'),
('Football Accessories', 'Whistles, pumps, and ball bags'),
('Football Training Kits', 'Training wear for football practice');


select * from Wishlist where UserId="10a6ba31-f01a-11ef-921f-18c04de083c5" and ProductId= "5ae5e5e1-f354-11ef-921f-18c04de083c5";
select * from Category;
DESC cart;
select CategoryName from Category order by CategoryName asc;

delete from wishlist where UserId="10afsnfghnsfhn11ef-921f-18c04de083c5@$$%#%^%&@$%#%^%$^Y&%^*" and ProductId= "rgsdrtnfnhfyhdf#%$&%$^%$^$%";
select CategoryId,CategoryName from Category order by CategoryName asc;