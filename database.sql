create database DBAuth

use DBAuth


create table Users(
 userId int primary key identity,
 name varchar(50),
 email varchar(50),
 password varchar(100)
)

create table Products (
	productId int primary key identity,
	name varchar(50),
	brandId int,
	price decimal(10,2)

)

create table Brands(
	brandId int primary key identity,
	name varchar(50)

)

/* Unir tabla brands con products */
alter table Products
add constraint fk_brandId
foreign key (brandId)
references Brands(brandId)

/* Insertar datos en la tabla Brands */
insert into Brands values('Nike')
insert into Brands values('Adidas')
insert into Brands values('Puma')
insert into Brands values('HP')
insert into Brands values('Samsung')
insert into Brands values('Apple')


/* Insertar datos en la tabla Products */
insert into Products values('Zapatillas', 1, 100)
insert into Products values('Polera', 2, 50)
insert into Products values('Pantalon', 3, 70)
insert into Products values('Laptop', 4, 500)
insert into Products values('Galaxy A15', 5, 500)
insert into Products values('Iphone 15 Pro Max', 6, 1200)


/* Consulta con inner join */

select * from Products inner join Brands on Products.brandId = brands.brandId
where Brands.brandId = 6

/* secret key jwt */
select NEWID()

select * from Users
select * from Products