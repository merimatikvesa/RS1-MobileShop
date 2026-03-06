USE MobileShopDB;
GO

DELETE FROM Inventory;
DELETE FROM ProductImages;
DELETE FROM Images;
DELETE FROM Products;
DELETE FROM Brands;
DELETE FROM Categories;
DELETE FROM Suppliers;

DBCC CHECKIDENT ('Brands', RESEED, 0);
DBCC CHECKIDENT ('Categories', RESEED, 0);
DBCC CHECKIDENT ('Suppliers', RESEED, 0);
DBCC CHECKIDENT ('Products', RESEED, 0);
DBCC CHECKIDENT ('Images', RESEED, 0);
DBCC CHECKIDENT ('ProductImages', RESEED, 0);
DBCC CHECKIDENT ('Accounts', RESEED, 0);
DBCC CHECKIDENT ('Administrators', RESEED, 0);

--ACCOUNTS
INSERT INTO dbo.Accounts(Username, [Password], FirstName, LastName) VALUES('admin@gmail.com', 'admin123.','Jane','Admin1'), ('user@gmail.com', 'user123.','Jack','User1')

--ADMINISTRATORS
INSERT INTO dbo.Administrators(AccountId) VALUES('1')

--USERS
INSERT INTO dbo.Users(Email,AccountId,FullName) VALUES('user@gmail.com','2','Jack User1')

-- BRANDS
INSERT INTO Brands (Name)
VALUES
('Apple'),
('Samsung'),
('Google'),
('Xiaomi'),
('OnePlus'),
('Huawei');

-- CATEGORY
INSERT INTO Categories (Name)
VALUES ('Smartphones'), ('Phone Cases'), ('Charger');



-- SUPPLIER
INSERT INTO Suppliers (SupplierName, PhoneNumber, Address)
VALUES
('Main Supplier', '+38761111222', 'Sarajevo, BiH'), ('Default Supplier', '+38761111222', 'Sarajevo, BiH');

-- PRODUCTS
INSERT INTO Products (ProductName, Model, Price, BrandId, SupplierId, CategoryId, PromotionId, CreatedAt)
VALUES
('iPhone 14', 'A2882', 990, 1, 1, 1, NULL, GETDATE()),
('iPhone 15', 'A3090', 1099, 1, 1, 1, NULL, GETDATE()),
('iPhone 15 Pro', 'A3101', 1290, 1, 1, 1, NULL, GETDATE()),

('Samsung Galaxy S23', 'SM-S911B', 899, 2, 1, 1, NULL, GETDATE()),
('Samsung Galaxy S24', 'SM-S921B', 960, 2, 1, 1, NULL, GETDATE()),
('Samsung Galaxy A54', 'SM-A546B', 440, 2, 1, 1, NULL, GETDATE()),

('Google Pixel 8', 'GZPF0', 795, 3, 1, 1, NULL, GETDATE()),

('Xiaomi 13', '2211133G', 694, 4, 1, 1, NULL, GETDATE()),
('Xiaomi 14', '23127PN0CC', 791, 4, 1, 1, NULL, GETDATE()),

('OnePlus 12', 'CPH2573', 859, 5, 1, 1, NULL, GETDATE()),

('Huawei P60', 'LNA-LX9', 800, 6, 1, 1, NULL, GETDATE()),
('Huawei Mate 50', 'DCO-LX9', 950, 6, 1, 1, NULL, GETDATE());




SET IDENTITY_INSERT Images ON;

INSERT INTO Images (ImageId, ImagePath) VALUES
(6,'/uploads/products/1/c84187dcd33941959eb9f49e103749d5.jpg'),
(7,'/uploads/products/1/8b03ce8acadd4fbcaa3c4992514995fa.jpg'),
(8,'/uploads/products/1/9de5298631d046348d17a8aed4c74388.jpg'),
(9,'/uploads/products/1/c431b976abaa487b9b4eda9547d2d3c5.jpg'),
(10,'/uploads/products/1/9bd6a441d64b41c185cd63ce9cf37a9b.jpg'),

(11,'/uploads/products/2/697c42ff98b84991895706482cf12c8d.jpg'),
(12,'/uploads/products/2/e4535bc2f2184ae2998999fe84048a78.jpg'),
(13,'/uploads/products/2/12064387cbf84bd7be7b2ddaca62ab37.jpg'),
(14,'/uploads/products/2/ad5c92a5a67042f3b5b78be69a97114d.jpg'),
(15,'/uploads/products/2/2cb7180e8fee42e98172c4328e41e511.jpg'),

(16,'/uploads/products/3/8b4d7c6098bd4ccca376b8ddf7be6813.jpg'),
(17,'/uploads/products/3/be4497ae5c844045a009ee37743ab801.jpg'),
(18,'/uploads/products/3/f6db00012f1c4e8b8c9c6eba290724d2.jpg'),
(19,'/uploads/products/3/7036756bfc254b6f8fea5ec10f8d565b.jpg'),
(20,'/uploads/products/3/f02f4951ec384ca2af289904aae7fd9f.jpg'),

(21,'/uploads/products/4/3485e6de00a74e0eb5aa37ca744fc2f6.jpg'),
(22,'/uploads/products/4/ae471ca3d2d6471e8a4054a835d7a68d.jpg'),
(23,'/uploads/products/4/5664f59460fe495ca911287512c02e9d.jpg'),
(24,'/uploads/products/4/98d950279ff7455298681881b34e4656.jpg'),
(25,'/uploads/products/4/48c413b27a7449079cf86cbb1b112a2e.jpg'),

(26,'/uploads/products/5/fc8b4c1158db4bf9807c1793486698df.jpg'),
(27,'/uploads/products/5/3648852f3b6644d0890cb0303aaffd36.jpg'),
(28,'/uploads/products/5/f62e532ffa9942acaea2c01b0b95d304.jpg'),
(29,'/uploads/products/5/c34cf046c44a4ffea6c85c492efe5d6a.jpg'),
(30,'/uploads/products/5/eda887afb50943c09278f8dfe8ffa6f1.jpg'),

(31,'/uploads/products/6/2c3c8a540a30456788d9d634a724f249.jpg'),
(32,'/uploads/products/6/8bbc9c9f6ade4cbba625fc98054e57d1.jpg'),
(33,'/uploads/products/6/42a658778b8a4dd5b8d3a2a0c872dd0b.jpg'),
(34,'/uploads/products/6/54a982c54fd543c38833dfb96d6b16c9.jpg'),
(35,'/uploads/products/6/6b5b7c6ec22242a9ba2bec36baee1439.jpg'),

(36,'/uploads/products/7/a3d058ee3fd243abba91c41a0493b1fa.jpg'),
(37,'/uploads/products/7/eeb0a50778dc4215a29900d32a0874c0.jpg'),

(38,'/uploads/products/8/75fadbb2c60e41c494f4f5b829fa8efc.jpg'),
(39,'/uploads/products/8/c0f5a050449d4bd8be5626dc61c9f5f7.jpg'),
(40,'/uploads/products/8/3afeffdcfb8a4c619b15f836b784aa2f.jpeg'),
(41,'/uploads/products/8/a04fa1f940c14c599309424156087457.jpg'),

(42,'/uploads/products/9/0000a318d18a47dea421b6e38755d364.jpg'),
(43,'/uploads/products/9/5b1dc5bd525f4cffa7e937a87ba5d2d8.jpg'),
(44,'/uploads/products/9/46e3f96cb69548aebe1e2aed2c23b717.jpg'),

(45,'/uploads/products/10/b95542ae72a44c199069976c93c65746.jpg'),
(46,'/uploads/products/10/024a992340c24e8d8318b0f1097fc151.jpg'),
(47,'/uploads/products/10/347088fb92c2442aabdc8ab7b52c39ec.jpg'),
(48,'/uploads/products/10/bd818b7a6fe8479f81e8218262c1bc91.jpg'),
(49,'/uploads/products/10/9bb3c5f012ae43f48aa9f51194539819.jpg'),

(50,'/uploads/products/11/20479e234bdc46c5a16a56d6a256324c.jpg'),
(51,'/uploads/products/11/b661b0112517415eb0b13e5a836baa3c.jpg'),

(52,'/uploads/products/12/40f4c28578d644278248438b366b42b1.jpg'),
(53,'/uploads/products/12/e66c81b4da3047e3a4889cc3f870bec5.jpg');

SET IDENTITY_INSERT Images OFF;



SET IDENTITY_INSERT ProductImages ON;

INSERT INTO ProductImages (ProductImageId, ImageId, ProductId) VALUES
(6,6,1),(7,7,1),(8,8,1),(9,9,1),(10,10,1),
(11,11,2),(12,12,2),(13,13,2),(14,14,2),(15,15,2),
(16,16,3),(17,17,3),(18,18,3),(19,19,3),(20,20,3),
(21,21,4),(22,22,4),(23,23,4),(24,24,4),(25,25,4),
(26,26,5),(27,27,5),(28,28,5),(29,29,5),(30,30,5),
(31,31,6),(32,32,6),(33,33,6),(34,34,6),(35,35,6),
(36,36,7),(37,37,7),
(38,38,8),(39,39,8),(40,40,8),(41,41,8),
(42,42,9),(43,43,9),(44,44,9),
(45,45,10),(46,46,10),(47,47,10),(48,48,10),(49,49,10),
(50,50,11),(51,51,11),
(52,52,12),(53,53,12);

SET IDENTITY_INSERT ProductImages OFF;