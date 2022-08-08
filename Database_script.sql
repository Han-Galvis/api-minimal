create database Crud_nodejs;
GO
use Crud_nodejs;
GO
--table user
Create table Users(
id int primary key identity,
first_name varchar(100),
last_name varchar(100),
phone varchar(50),
email varchar(100)
);
GO
