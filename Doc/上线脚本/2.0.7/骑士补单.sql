USE superman
go

ALTER TABLE dbo.[OrderOther]
ADD OneKeyPubOrder INT DEFAULT 0 NOT NULL
GO

UPDATE  oo
SET     oo.OneKeyPubOrder = 1
FROM    dbo.OrderOther oo ( NOLOCK )
        JOIN dbo.[order] o ( NOLOCK ) ON oo.OrderId = o.Id
WHERE   ( ISNULL(ReceviceAddress, '') = ''
          OR ISNULL(RecevicePhoneNo, '') = ''
        )
        AND pubdate > '2015-05-24' 