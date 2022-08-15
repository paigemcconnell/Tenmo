USE master
GO

IF DB_ID('tenmo') IS NOT NULL
BEGIN
	ALTER DATABASE tenmo SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE tenmo;
END

CREATE DATABASE tenmo
GO

USE tenmo
GO

CREATE TABLE transfer_types (
	transfer_type_id int IDENTITY(1000,1) NOT NULL,
	transfer_type_desc varchar(10) NOT NULL,
	CONSTRAINT PK_transfer_types PRIMARY KEY (transfer_type_id)
)

CREATE TABLE transfer_statuses (
	transfer_status_id int IDENTITY(2000,1) NOT NULL,
	transfer_status_desc varchar(10) NOT NULL,
	CONSTRAINT PK_transfer_statuses PRIMARY KEY (transfer_status_id)
)

CREATE TABLE users (
	user_id int IDENTITY(3000,1) NOT NULL,
	username varchar(50) NOT NULL,
	password_hash varchar(200) NOT NULL,
	salt varchar(200) NOT NULL,
	CONSTRAINT PK_user PRIMARY KEY (user_id)
)

CREATE TABLE accounts (
	account_id int IDENTITY(4000,1) NOT NULL,
	user_id int NOT NULL,
	balance decimal(13, 2) NOT NULL,
	CONSTRAINT PK_accounts PRIMARY KEY (account_id),
	CONSTRAINT FK_accounts_user FOREIGN KEY (user_id) REFERENCES users (user_id)
)

CREATE TABLE transfers (
	transfer_id int IDENTITY(5000,1) NOT NULL,
	transfer_type_id int NOT NULL,
	transfer_status_id int NOT NULL,
	account_from int NOT NULL,
	account_to int NOT NULL,
	amount decimal(13, 2) NOT NULL,
	CONSTRAINT PK_transfers PRIMARY KEY (transfer_id),
	CONSTRAINT FK_transfers_accounts_from FOREIGN KEY (account_from) REFERENCES accounts (account_id),
	CONSTRAINT FK_transfers_accounts_to FOREIGN KEY (account_to) REFERENCES accounts (account_id),
	CONSTRAINT FK_transfers_transfer_statuses FOREIGN KEY (transfer_status_id) REFERENCES transfer_statuses (transfer_status_id),
	CONSTRAINT FK_transfers_transfer_types FOREIGN KEY (transfer_type_id) REFERENCES transfer_types (transfer_type_id),
	CONSTRAINT CK_transfers_not_same_account CHECK  ((account_from<>account_to)),
	CONSTRAINT CK_transfers_amount_gt_0 CHECK ((amount>0))
)


INSERT INTO transfer_statuses (transfer_status_desc) VALUES ('Pending');
INSERT INTO transfer_statuses (transfer_status_desc) VALUES ('Approved');
INSERT INTO transfer_statuses (transfer_status_desc) VALUES ('Rejected');

INSERT INTO transfer_types (transfer_type_desc) VALUES ('Request');
INSERT INTO transfer_types (transfer_type_desc) VALUES ('Send');

select username, user_id from users 

select * from transfers
BEGIN TRANSACTION

INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount)
Values (1001, 2001, 4000, 4001, 50)

UPDATE accounts 
SET balance = 
	(select accounts.balance + transfers.amount as [CurrentBalance]
	from accounts join transfers on accounts.account_id = transfers.account_from 
	where transfer_id = @@IDENTITY)
WHERE account_id = 4001

Select * From accounts 

ROLLBACK TRANSACTION

Select * From Transfers 

--subquery account from 
select u.username
from users u
	inner join accounts a on a.user_id = u.user_id
	inner join transfers t on t.account_from = a.account_id

--subquery account to
select u.username
from users u
	inner join accounts a on a.user_id = u.user_id
	inner join transfers t on t.account_to = a.account_id


--please work 
select t.transfer_id, t.account_from, (select u.username
from users u
	inner join accounts a on a.user_id = u.user_id
	inner join transfers t on t.account_from = a.account_id) as 'From' , t.account_to, (select u.username
from users u
	inner join accounts a on a.user_id = u.user_id
	inner join transfers t on t.account_to = a.account_id) as 'To', t.amount
From transfers t
		inner join accounts a on a.account_id = t.account_from
		inner join users u on u.user_id = a.user_id

Union all

select t.transfer_id, t.account_from, (select u.username
from users u
	inner join accounts a on a.user_id = u.user_id
	inner join transfers t on t.account_from = a.account_id) as 'From' , t.account_to, (select u.username
from users u
	inner join accounts a on a.user_id = u.user_id
	inner join transfers t on t.account_to = a.account_id) as 'To', t.amount
From transfers t
		inner join accounts a on a.account_id = t.account_to
		inner join users u on u.user_id = a.user_id




select t.transfer_id, t.account_from, u.username as 'From' , t.account_to, u.username as 'To', t.amount
From transfers t
		inner join accounts a on a.account_id = t.account_from  OR a.account_id = t.account_to
		inner join users u on u.user_id = a.user_id

Union 

select t.transfer_id, t.account_from, u.username as 'From' , t.account_to, u.username as 'To', t.amount
From transfers t
		inner join accounts a on a.account_id = t.account_to OR a.account_id = t.account_to
		inner join users u on u.user_id = a.user_id


select * from accounts


select * from transfers


SELECT t.transfer_id, t.account_from, uFrom.username AS 'From', t.account_to, uTo.username AS 'To', t.amount 
                FROM transfers t INNER JOIN accounts aTo ON aTo.account_id = t.account_to INNER JOIN users uTo ON uTo.user_id = aTo.user_id 
                INNER JOIN accounts aFrom ON aFrom.account_id = t.account_from INNER JOIN users uFrom ON uFrom.user_id = aFrom.user_id 
                WHERE t.transfer_id = 5001
				
				INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount)
Values (1001, 2001, 4000, 4001, 50)



				UPDATE accounts SET balance = (select balance + amount as [CurrentBalance] 
                from accounts join transfers  on account_id = account_from where transfer_id = @@IDENTITY) 
                WHERE 
Select * From transfers

Select * from accounts
				