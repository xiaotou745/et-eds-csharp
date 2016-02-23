USE [superman]
GO
/****** Object:  StoredProcedure [dbo].[Sp_CustomPage2015_V2]    Script Date: 02/22/2016 16:16:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--֧��GroupBy�ķ�ҳ��ѯ
ALTER PROC [dbo].[Sp_CustomPage2015_V2]
--SqlServer2008  �кŷ�ҳ  V1.2   ����ȫ�Ĳ�ѯ����������ֹע��

@OrderByColumn varchar(max),--�����ֶ���
@ColumnList varchar(max),--���ص��ֶ��б�
@TableList varchar(max),--����������ŷָ�
@Condition varchar(max),--��ѯ����
@GroupBy varchar(max)='',--GroupBy����--����Ҫ��ʱ��''
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
	DECLARE @temp2 NVARCHAR(max)
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