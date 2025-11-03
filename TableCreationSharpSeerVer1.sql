create table Cohorts(
ID int primary key identity(1,1),
Name varchar(23) unique not null,
Major varchar(31) not null,
Term int not null
);

create table Teachers(
ID int primary key identity(1,1),
Name varchar(255) not null,
Email varchar(255) unique not null,
PhoneNumber varchar(15)
);


create table Exams(
ID int primary key identity(1,1),
Name varchar(255) not null,
CohortID int foreign key references Cohorts(ID) unique not null,
IsGuarded bit not null default 0,
NeedsExternalExaminer bit not null default 0,
FirstExamDate datetime2 not null,
LastExamDate datetime2 not null,
HandInDeadline datetime2,
ExamDurationMinutes int not null,
ExamType int not null
);

create table Examiners(
ExamId int foreign key references Exams(ID),
TeacherId int foreign key references Teachers(ID),
primary key(ExamId, TeacherId)
);