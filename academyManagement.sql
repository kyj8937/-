
create database if not exists academyManagement;

show databases;

use academyManagement;

drop table if exists Course;
drop table if exists Student;
drop table if exists Register;
drop table if exists Attendance;

create table if not exists Course(
	courseNum varchar(6) primary key,
    courseName varchar(30),
    classTime varchar(20),  
    teacher varchar(10),	  
	overView varchar(200),
    classDuration integer
);

create table if not exists Student(
	id varchar(20) primary key,
    name varchar(30),
    identificationNum varchar(20),
    address varchar(40),
    phoneNum varchar(20),
    email varchar(40)
);

create table if not exists Register(
    id varchar(20),
    courseNum varchar(6),
    registerDate Date,
    primary key(id, courseNum),
    foreign key (id) references Student(id),
    foreign key (courseNum) references Course(courseNum)
);

create table if not exists Attendance(
	id varchar(30),
    date Date,  
    attend boolean,
	primary key(id, date),
    foreign key (id) references Student(id)
);

select * from Course;
select * from Student;
select * from Register;
select * from Attendance;

set global local_infile=true;
load data local infile 'C:/textFile/Course.txt' into table Course fields terminated by '|' lines terminated by '\n';
load data local infile 'C:/textFile/Student.txt' into table Student fields terminated by '#' lines terminated by '\n';
load data local infile 'C:/textFile/Register.txt' into table Register fields terminated by ',' lines terminated by '\n';
load data local infile 'C:/textFile/Attendance.txt' into table Attendance fields terminated by ',' lines terminated by '\n';



