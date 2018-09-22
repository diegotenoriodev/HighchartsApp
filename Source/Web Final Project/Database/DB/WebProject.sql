CREATE DATABASE Forever16DB
GO
USE Forever16DB
GO
CREATE TABLE Client
(
	Id	INT NOT NULL IDENTITY(1, 1),
	FirstName VARCHAR(64) NOT NULL,
	LastName VARCHAR(64) NOT NULL,
	[Address] VARCHAR(64) NOT NULL,
	DateOfBirth DATE NOT NULL,
	gender varchar(16) NOT NULL,
	CONSTRAINT PK_CLIENT PRIMARY KEY (ID)
)
GO
CREATE TABLE Product 
(
	Id INT NOT NULL IDENTITY(1, 1),
	[Name] VARCHAR(64) NOT NULL,
	Price MONEY NOT NULL,
	QttAvailable INT NOT NULL,
	CONSTRAINT PK_PRODUCT PRIMARY KEY (Id)
)
GO
CREATE TABLE Store
(
	Id INT NOT NULL IDENTITY(1, 1),
	[Name] VARCHAR(64) NOT NULL,
	[Address] VARCHAR(64) NOT NULL,
	City VARCHAR(64) NOT NULL,
	CONSTRAINT PK_STORE PRIMARY KEY (ID)
)
GO
CREATE TABLE Sale
(
	Id INT NOT NULL IDENTITY(1, 1),
	[Date] DATETIME NOT NULL,
	PaymentType INT NOT NULL, --1 FOR CREDIT CARD, 2 FOR CASH, 3 FOR DEBIT CARD
	ClientId INT NOT NULL,
	StoreId INT NOT NULL,
	CONSTRAINT PK_SALE PRIMARY KEY (ID),
	CONSTRAINT FK_SALE_CLIENT FOREIGN KEY (ClientId) REFERENCES Client(Id),
	CONSTRAINT FK_SALE_STORE FOREIGN KEY (StoreId) REFERENCES Store(Id)
)
GO
CREATE TABLE SaleItem
(
	ID INT NOT NULL IDENTITY(1, 1),
	ProductId INT NOT NULL,
	SaleId INT NOT NULL,
	Quantity INT NOT NULL
	CONSTRAINT PK_SALEITEM PRIMARY KEY (ID),
	CONSTRAINT FK_SALEITEM_PRODUCT FOREIGN KEY (ProductId) REFERENCES Product(Id),
	CONSTRAINT FK_SALEITEM_SALE FOREIGN KEY (SaleId) REFERENCES Sale(Id)
)
GO

/*Seeding*/

INSERT INTO Product([Name], Price, QttAvailable) values ('Suit', 200.0, 500);
INSERT INTO Product([Name], Price, QttAvailable) values ('Shirt', 24.2, 500);
INSERT INTO Product([Name], Price, QttAvailable) values ('Tie', 11.7, 500);
INSERT INTO Product([Name], Price, QttAvailable) values ('Dress', 55.2, 500);
INSERT INTO Product([Name], Price, QttAvailable) values ('Skirt', 12.2, 500);
INSERT INTO Product([Name], Price, QttAvailable) values ('Tanktop', 152.66, 500);
INSERT INTO Product([Name], Price, QttAvailable) values ('Coat', 34.5, 500);
INSERT INTO Product([Name], Price, QttAvailable) values ('Jacket', 55.11, 500);
INSERT INTO Product([Name], Price, QttAvailable) values ('Sweatshirt', 231.99, 500);
INSERT INTO Product([Name], Price, QttAvailable) values ('Trouser', 32.1, 500);
INSERT INTO Product([Name], Price, QttAvailable) values ('T-shirt', 22.54, 500);
INSERT INTO Product([Name], Price, QttAvailable) values ('Short', 12.66, 500);
INSERT INTO Product([Name], Price, QttAvailable) values ('Boxer', 12.4, 500);
INSERT INTO Product([Name], Price, QttAvailable) values ('Bra', 17.65, 500);
INSERT INTO Product([Name], Price, QttAvailable) values ('Pant', 11.12, 500);
INSERT INTO Product([Name], Price, QttAvailable) values ('Shoes', 44.66, 500);
INSERT INTO Product([Name], Price, QttAvailable) values ('Sandal', 9.55, 500);
INSERT INTO Product([Name], Price, QttAvailable) values ('Boot', 67.44, 500);
INSERT INTO Product([Name], Price, QttAvailable) values ('Slipper', 12.58, 500);
INSERT INTO Product([Name], Price, QttAvailable) values ('Sock', 31.0, 500);


INSERT INTO Store([NAME], [Address], City) VALUES ('Fairview', '2960 Kingsway Dr', 'Kitchener')
INSERT INTO Store([NAME], [Address], City) VALUES ('Conestoga Mall', '550 King St N', 'Waterloo')
INSERT INTO Store([NAME], [Address], City) VALUES ('Sunrise Shopping Centre', '1400 Ottawa St S', 'Kitchener')
INSERT INTO Store([NAME], [Address], City) VALUES ('Cambridge Centre', '355 Hespeler Rd', 'Cambridge')
INSERT INTO Store([NAME], [Address], City) VALUES ('Toronto Eaton Centre', '220 Yonge St', 'Toronto')

