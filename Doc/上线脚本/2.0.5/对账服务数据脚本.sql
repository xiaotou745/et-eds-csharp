CREATE TABLE "ClienterAccountChecking" (
	"Id" INT NOT NULL identity(1,1),
	"ClienterId" INT NOT NULL,
	"CreateDate" DATETIME NOT NULL,
	"FlowStatMoney" DECIMAL(18,2) NOT NULL,
	"ClienterTotalMoney" DECIMAL(18,2) NOT NULL,
	"StartDate" DATETIME NOT NULL,
	"EndDate" DATETIME NOT NULL,
	PRIMARY KEY ("Id")
);
alter table ClienterAccountChecking
	add LastTotalMoney DECIMAL(18,2) default 0 NOT NULL;