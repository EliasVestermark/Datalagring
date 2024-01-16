CREATE TABlE Categories
(
	CategoryId int not null identity primary key,
	Name nvarchar(20)
)

CREATE TABlE Ingridients
(
	IngridientId int not null identity primary key,
	Name nvarchar(50)
)
GO

CREATE TABLE Products
(
	ProductId int not null identity primary key,
	Name nvarchar(50) not null,
	Price money not null, 
	CategoryId int not null references Categories(CategoryId)
)
GO

CREATE TABLE ProductIngridients
(
	ProductId int not null references Products(ProductId),
	IngridientId int not null references Ingridients(IngridientId)

	primary key (ProductId, IngridientId)
)