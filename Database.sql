CREATE DATABASE Storage

Use Storage

Create table Product
(
	[Id] INT primary key,
	[Name] nvarchar(50) not null,
	[TypeId] int not null,
	[Count] int not null,
	[CostPrice] money not null,
	[DateOfDelivery] date not null
);

Create table ProdType
(
	[Id] int primary key,
	[Name] nvarchar(50) not null
);

Create table ProductToProvider
(
	[Id] int primary key,
	[ProviderId] int not null,
	[ProductId] int not null
);

Create table ProdProvider
(
	[Id] int primary key,
	[Name] nvarchar(50) not null
);