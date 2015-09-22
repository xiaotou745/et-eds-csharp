USE [superman]
GO
/****** Object:  StoredProcedure [dbo].[Sp_CustomPage2015_V2]    Script Date: 09/17/2015 16:35:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--支持GroupBy的分页查询
CREATE PROC [dbo].[Sp_CustomPage2015_V2]
--SqlServer2008  行号分页  V1.2   增加全文查询参数化，防止注入

@OrderByColumn varchar(8000),--排序字段名
@ColumnList varchar(max),--返回的字段列表
@TableList varchar(8000),--表名，多表逗号分隔
@Condition varchar(8000),--查询条件
@GroupBy varchar(8000)='',--GroupBy条件--不需要的时候穿''
@PageSize int,
@CurrentPage int,
@IsAccount bit,
@TotalRecord int output,
@TotalPage int output
AS
set @TotalRecord=0;
set @TotalPage=0;
--计算记录数
IF @IsAccount = 1
BEGIN
	DECLARE @temp2 NVARCHAR(2000)
	--构造获得总页的数的检索语句
	IF(@GroupBy='')
		BEGIN
			set @temp2=N'select @total2=count(1) from ' + @TableList + ' where ' + @Condition
		END
	ELSE
		BEGIN
			set @temp2=N'select @total2= ISNULL(SUM(T2.[No]),0) FROM( SELECT 1 AS [No] FROM  ' + @TableList + ' where ' + @Condition +' GROUP BY '+@GroupBy +' ) AS T2'
		END
	
	PRINT(@temp2)
	--执行检索语句,取得总的记录条数
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