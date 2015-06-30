
USE superman
go

ALTER TABLE dbo.[order]
ADD OneKeyPubOrder INT DEFAULT 0 NOT NULL
go

UPDATE dbo.[order] SET OneKeyPubOrder=1 WHERE (ISNULL(ReceviceAddress,'')='' or ISNULL(RecevicePhoneNo,'')='') and pubdate>'2015-05-24' 



