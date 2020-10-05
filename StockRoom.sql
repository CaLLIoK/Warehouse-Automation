  create database StockRoom

  use StockRoom

  create table Users
  (UserCode int not null identity,
  UserLogin varchar(20) not null,
  UserPassword varchar(20) not null,
  AdministratorState bit not null default 0,
  UserSurname varchar(20) null,
  UserName varchar(20) null, 
  constraint UserLoginUniq unique (UserLogin),
  constraint UserCode_PK primary key (UserCode))
  insert into Users (UserLogin, UserPassword, AdministratorState, UserName, UserSurname) values ('admin1', 'wrldftnks', 1, 'Иван', 'Петров')
  insert into Users (UserLogin, UserPassword, AdministratorState, UserName, UserSurname) values ('admin2', 'bttlfld', 1, 'Александр', 'Самсонов')
  insert into Users (UserLogin, UserPassword, AdministratorState, UserName, UserSurname) values ('admin3', 'strwrs_bttlfrnt', 1, 'Евгений', 'Грамович')
  insert into Users (UserLogin, UserPassword, AdministratorState, UserName, UserSurname) values ('admin', 'qwerty', 1, 'Дарья', 'Кострова')

  create table CarModel
  (IDCarModel int not null identity,
  CarModelName varchar(30) not null,
  constraint CarModelNameCheck unique (IDCarModel),
  constraint IDCarModel_PK primary key (IDCarModel))
  insert into CarModel (CarModelName) values ('Mercedes'), ('BMW'), ('Mitsubishi'), ('KIA'), ('Reno'), ('Opel')

  create table SparePartName
  (IDSparePartN int not null identity,
  SparePartN varchar(30) not null,
  constraint SparePartNameCheck unique (IDSparePartN),
  constraint IDSparePartName_PK primary key (IDSparePartN))
  insert into SparePartName (SparePartN) values ('Аккумулятор'), ('Втулка стабилизатора'), ('Наружное зеркало'), ('Масляной фильтр'), ('Датчик ABS'), ('Воздушный фильтр')

  create table SparePartStatus
  (IDStatus int not null identity,
  StateName varchar(14),
  constraint StateNameCheck unique (StateName),
  constraint IDState_PK primary key (IDStatus))
  insert into SparePartStatus (StateName) values ('Есть в наличии'), ('Нет в наличии')

  create table SparePart
  (SparePartNumber int not null identity,
  IDSparePartN int not null,
  IDCarModel int not null,
  SparePartCreation date not null,
  SparePartCost float not null, 
  SparePartCount int not null, 
  IDStatus int not null,
  constraint IDSparePartName_FK foreign key (IDSparePartN) references SparePartName (IDSparePartN),
  constraint IDCarModel_FK foreign key (IDCarModel) references CarModel (IDCarModel),
  constraint IDStatus_FK foreign key (IDStatus) references SparePartStatus (IDStatus),
  constraint SparePartNumber_PK primary key (SparePartNumber))
  insert into SparePart (IDCarModel, IDSparePartN, SparePartCount, SparePartCreation, SparePartCost, IDStatus) values (1, 1, 12, '02.08.2019', 98.00, 1)
  insert into SparePart (IDCarModel, IDSparePartN, SparePartCount, SparePartCreation, SparePartCost, IDStatus) values (2, 2, 19, '14.03.2020', 3.00, 1)
  insert into SparePart (IDCarModel, IDSparePartN, SparePartCount, SparePartCreation, SparePartCost, IDStatus) values (3, 3, 9, '07.01.2018', 73.00, 1)
  insert into SparePart (IDCarModel, IDSparePartN, SparePartCount, SparePartCreation, SparePartCost, IDStatus) values (4, 4, 5, '09.12.2019', 11.00, 1)
  insert into SparePart (IDCarModel, IDSparePartN, SparePartCount, SparePartCreation, SparePartCost, IDStatus) values (5, 5, 11, '14.03.2020', 38.00, 1)
  insert into SparePart (IDCarModel, IDSparePartN, SparePartCount, SparePartCreation, SparePartCost, IDStatus) values (6, 6, 3, '01.04.2020', 12.00, 1)
  insert into SparePart (IDCarModel, IDSparePartN, SparePartCount, SparePartCreation, SparePartCost, IDStatus) values (2, 5, 11, '11.07.2020', 38.00, 1)
  insert into SparePart (IDCarModel, IDSparePartN, SparePartCount, SparePartCreation, SparePartCost, IDStatus) values (4, 6, 3, '01.04.2020', 12.00, 1)
  insert into SparePart (IDCarModel, IDSparePartN, SparePartCount, SparePartCreation, SparePartCost, IDStatus) values (5, 3, 9, '08.10.2018', 73.00, 1)
  --delete from SparePart
  --drop table SparePart

  create table SparePartOrder
  (OrderNumber int not null identity,
  UserCode int not null,
  SparePartNumber int not null,
  SparePartCount int not null,
  GeneralSum float not null,
  constraint OrderNumber_PK primary key (OrderNumber),
  constraint SparePartNumber_FK foreign key (SparePartNumber) references SparePart (SparePartNumber) on delete cascade,
  constraint UserCode_FK foreign key (UserCode) references Users (UserCode) on delete cascade)
  --drop table SparePartOrder
  --delete from SparePartOrder