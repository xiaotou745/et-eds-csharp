USE [superman]
GO
/****** Object:  StoredProcedure [dbo].[Sp_CustomPage2015_V2]    Script Date: 09/17/2015 16:35:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--֧��GroupBy�ķ�ҳ��ѯ
CREATE PROC [dbo].[Sp_CustomPage2015_V2]
--SqlServer2008  �кŷ�ҳ  V1.2   ����ȫ�Ĳ�ѯ����������ֹע��

@OrderByColumn varchar(8000),--�����ֶ���
@ColumnList varchar(max),--���ص��ֶ��б�
@TableList varchar(8000),--����������ŷָ�
@Condition varchar(8000),--��ѯ����
@GroupBy varchar(8000)='',--GroupBy����--����Ҫ��ʱ��''
@PageSize int,
@CurrentPage int,
@IsAccount bit,
@TotalRecord int output,
@TotalPage int output
AS
set @TotalRecord=0;
set @TotalPage=0;
--�����¼��
IF @IsAccount = 1
BEGIN
	DECLARE @temp2 NVARCHAR(2000)
	--��������ҳ�����ļ������
	IF(@GroupBy='')
		BEGIN
			set @temp2=N'select @total2=count(1) from ' + @TableList + ' where ' + @Condition
		END
	ELSE
		BEGIN
			set @temp2=N'select @total2= ISNULL(SUM(T2.[No]),0) FROM( SELECT 1 AS [No] FROM  ' + @TableList + ' where ' + @Condition +' GROUP BY '+@GroupBy +' ) AS T2'
		END
	
	PRINT(@temp2)
	--ִ�м������,ȡ���ܵļ�¼����
	exec sp_executesql @temp2,N' @total2 int output ',@TotalRecord OUTPUT
	
	IF @TotalRecord > 0
	BEGIN
		set @TotalPage = (@TotalRecord+@PageSize-1)/@PageSize
	END
END
ELSE
BEGIN
	SET @TotalRecord = 0
	SET @TotalPage = 0
END

DECLARE @StartRowNum INT
SET @StartRowNum = @PageSize*(@CurrentPage-1) + 1
DECLARE @EndRowNum INT
SET @EndRowNum = @StartRowNum + @PageSize - 1
DECLARE @sql NVARCHAR(max)
	IF(@GroupBy='')
		BEGIN
			SET @sql = N'SELECT * FROM ( SELECT ' + @ColumnList + ',ROW_NUMBER() OVER (order by ' + @OrderByColumn + ' ) AS RowNum FROM ' + @TableList +' WHERE ' + @Condition + ' ) AS T WHERE RowNum BETWEEN ' + CAST(@StartRowNum AS VARCHAR(50)) + ' AND ' + CAST(@EndRowNum AS VARCHAR(50))
		END
	ELSE
		BEGIN
			SET @sql = N'SELECT * FROM ( SELECT ' + @ColumnList + ',ROW_NUMBER() OVER (order by ' + @OrderByColumn + ' ) AS RowNum FROM ' + @TableList +' WHERE ' + @Condition  +' GROUP BY '+ @GroupBy+' ) AS T WHERE RowNum BETWEEN ' + CAST(@StartRowNum AS VARCHAR(50)) + ' AND ' + CAST(@EndRowNum AS VARCHAR(50))
		END


PRINT @sql

	exec sp_executesql @sql