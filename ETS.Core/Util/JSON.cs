using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Script.Serialization;

namespace Letao.Util
{
    public static class JSON
    {
        private const int QUOT = 0;
        private const int LBRACE = 1;
        private const int RBRACE = 2;
        private const int LBRACK = 3;
        private const int RBRACK = 6;


        /// <summary>
        /// 把一个JSON格式的字符串转化为C#的对像
        /// 目前支持：整形、浮点、日期、数组(ArrayList)、表(DataTable)
        /// 注：ArrayList不支持多级 DataTable同样;也就是说ArrayList 成员不能是ArrayList或DataTable ,DataTable不能是DataTable或ArrayList
        /// </summary>
        /// <param name="inStr">输入的JSON格式的字符串</param>
        /// <returns>返回的C#数据类型，需要根据具体情况进行重新定义type</returns>
        public static Object fromJSON(String inStr)
        {
            return convertFromJSON(getToken(inStr));
        }



        private static JavaScriptSerializer objSerializer = new JavaScriptSerializer();
        /// <summary>
        /// datatable  转json,新版报表用
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt)
        {
            if (dt == null)
            {
                return "[]";
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    builder.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        builder.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + objSerializer.Serialize(dt.Rows[i][j]));
                        if (j < (dt.Columns.Count - 1))
                        {
                            builder.Append(",");
                        }
                    }
                    builder.Append("}");
                    if (i < (dt.Rows.Count - 1))
                    {
                        builder.Append(",");
                    }
                }
            }
            builder.Append("]");
            return builder.ToString();
        }


        /// <summary>
        /// 把C#对像转换为JSON数据格式（重载） 
        /// 目前支持：整形、浮点、日期、数组(ArrayList)、表(DataTable)
        ///  注：ArrayList不支持多级 DataTable同样;也就是说ArrayList 成员不能是ArrayList或DataTable ,DataTable不能是DataTable或ArrayList
        /// </summary>
        /// <param name="jobj">要转换成JSON字符串的对象</param>
        /// <param name="ifAddBackSlash">在每个反斜杠后面增加一个反斜杠，用来在页面直接打出DataTable时候用</param>
        /// <returns>转换后的JSON字符串</returns>

        public static string toJSON(Object jobj, bool ifAddBackSlash)
        {
            string s = toJSON(jobj);


            if (ifAddBackSlash && s != null)
                return s.Replace("\\", "\\\\");
            else
                return s;

        }

        /// <summary>
        /// 把C#对像转换为JSON数据格式
        /// 目前支持：整形、浮点、日期、数组(ArrayList)、表(DataTable)
        ///  注：ArrayList不支持多级 DataTable同样;也就是说ArrayList 成员不能是ArrayList或DataTable ,DataTable不能是DataTable或ArrayList
        /// </summary>
        /// <param name="jobj">要转换成JSON字符串的对象</param>
        /// <returns>转换后的JSON字符串</returns>
        public static string toJSON(Object jobj)
        {
            if (jobj == null)
                return "null";

            StringBuilder jResult = new StringBuilder();


            String sType = jobj.GetType().ToString();

            if (sType != "System.Collections.ArrayList")
                return Obj2JSON(jobj);
            else
            {
                ArrayList al = (ArrayList)jobj;
                jResult.Append('[');
                for (int i = 0; i < al.Count; i++)
                {
                    jResult.Append(Obj2JSON(al[i]));

                    if (i < al.Count - 1)
                        jResult.Append(',');
                }

                jResult.Append(']');
            }

            return jResult.ToString();
        }



        private static Object convertFromJSON(Object jObj)  //初步被解析过的ArrayList或者Obj被送到这里来，解析为C#的对象，number，datetime，arraylist，等
        {
            if (jObj == null)
                return null;                                //null应该是出了什么问题了，格式错误等。

            String s = jObj.GetType().ToString();           //如果不是null，那就首先判断一下对象的类型


            if (s != "System.Collections.ArrayList")        //对象不是ArrayList，那就是number之类东西，直接返回。
                return jObj;



            ArrayList al = (ArrayList)jObj;                 //下来判断输入值是不是一个单个对象，目前仅支持System.DataTable和System.DateTime */

            if (al[0] != null && al[0].GetType().ToString() == "System.Collections.ArrayList")
            {
                if (((ArrayList)al[0])[0].GetType().ToString() == "System.String")
                {
                    if ((String)(((ArrayList)al[0])[0]) == "DataType")
                        if ((String)(((ArrayList)al[0])[1]) == "System.DataTable")
                            return al2DataTable(al);
                        else if ((String)(((ArrayList)al[0])[1]) == "System.DateTime")
                            return al2DateTime(al);
                }
            }


            /* 输入值是一个列表，则逐个处理，逐个返回*/
            ArrayList tmpal, tmpal2;
            for (int i = 0; i < al.Count; i++)
            {
                if (al[i] == null)
                    continue;

                String sType = al[i].GetType().ToString();

                if (sType == "System.Collections.ArrayList")
                {
                    tmpal = (ArrayList)al[i];
                    tmpal2 = (ArrayList)tmpal[0];

                    if ((String)tmpal2[0] == "DataType")
                    {
                        if ((String)tmpal2[1] == "System.DataTable")
                            al[i] = al2DataTable(tmpal);
                        else if ((String)tmpal2[1] == "System.DateTime")
                            al[i] = al2DateTime(tmpal);
                    }
                }
            }

            return al;
        }

        /// <summary>
        /// ArrayList转换
        /// </summary>
        /// <param name="al"></param>
        /// <returns></returns>
        private static DataTable al2DataTable(ArrayList al)
        {
            DataTable dt = new DataTable();
            dt.TableName = (String)((ArrayList)(ArrayList)al[1])[1];
            int i, j;

            String columnName, columnType;
            ArrayList alColumns = (ArrayList)al[2];
            ArrayList colTemp;
            int columnsCount = ((ArrayList)alColumns[1]).Count;
            DataColumn dc;

            for (i = 0; i < columnsCount; i++)
            {
                colTemp = (ArrayList)((ArrayList)alColumns[1])[i];

                columnName = (String)((ArrayList)(ArrayList)colTemp[0])[1];
                columnType = (String)((ArrayList)(ArrayList)colTemp[1])[1];

                dc = new DataColumn(columnName, System.Type.GetType(columnType));

                dt.Columns.Add(dc);
            }

            ArrayList alRows = (ArrayList)al[3];

            ArrayList rowTemp;

            int rowsCount = ((ArrayList)alRows[1]).Count;
            String RowName = null;
            Object RowValue = null;
            DataRow dr;

            for (i = 0; i < rowsCount; i++)
            {
                dr = dt.NewRow();
                rowTemp = (ArrayList)((ArrayList)alRows[1])[i];

                for (j = 0; j < rowTemp.Count; j++)
                {
                    var rowTemp1 = ((ArrayList)rowTemp[j]);
                    if (rowTemp1 != null && rowTemp1.Count >= 2)
                    {
                        RowName = (String)rowTemp1[0];
                        RowValue = rowTemp1[1];
                    }

                    if (RowValue != null && RowValue.GetType().ToString() == "System.Collections.ArrayList")//DataTime
                        RowValue = al2DateTime((ArrayList)RowValue);
                    if (!string.IsNullOrEmpty(RowName))
                    {
                        if (RowValue == null)
                            dr[RowName] = DBNull.Value;
                        else
                            dr[RowName] = RowValue;
                    }
                }
                if (!string.IsNullOrEmpty(RowName))
                {
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }
        /// <summary>
        /// ArrayList转换为DateTime类型
        /// </summary>
        /// <param name="al"></param>
        /// <returns></returns>
        private static DateTime al2DateTime(ArrayList al)
        {
            if (al != null && al.Count >= 2)
            {
                var al1 = (ArrayList)al[1];
                if (al1 != null && al1.Count >= 2)
                {
                    if ((String)(al1)[0] == "Value")
                    {
                        String ts = (String)(al1)[1];
                        DateTime dte = DateTime.MinValue;
                        DateTime.TryParse(ts, out dte);
                        return dte;
                    }
                }
            }

            return DateTime.MinValue;  //出错啦，返回空

        }



        /// <summary>
        /// 将一个JSON字符串进行转化，可能是[,{,"，甚至什么都没开头。
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        private static Object getToken(String inStr)  //
        {
            //Object 的返回值可能是int,bool,string或者ArrayList。其中ArrayList包括了Array，Object两种类型
            char[] jstr = inStr.ToCharArray();

            int p = 0;

            while (p < jstr.Length)
            {
                if (jstr[p] == '{')
                    return getObj(inStr.Substring(p));
                else if (jstr[p] == '[')
                    return getList(inStr.Substring(p));
                else if (jstr[p] == '"')
                    return getString(inStr.Substring(p));
                else if (isSpace(jstr[p]))
                    p++;
                else
                    return getSingle(inStr.Substring(p));
            }

            if (p == jstr.Length)
            {
                //全都是空格，应该throw一个expection
                return null;
            }

            return null;
        }

        /// <summary>
        /// 将一个非{["开头的字符串换成对象，有可能是null，true/false还有数字。
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        private static Object getSingle(String inStr)  //
        {
            if (inStr == "null")
                return null;
            else if (inStr == "true" || inStr == "True")
                return true;
            else if (inStr == "false" || inStr == "False")
                return false;
            else if (inStr == "DBNull")
                return DBNull.Value;
            else
                return getNumber(inStr);

        }
        /// <summary>
        /// 把一个对像转换为一个数字
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        private static Object getNumber(String inStr)  //Process number
        {
            if (inStr.IndexOf('.') != -1 ||
                inStr.IndexOf('e') != -1 ||
                inStr.IndexOf('E') != -1)
            {
                Decimal n_decimal;
                if (Decimal.TryParse(inStr, out n_decimal))
                    return n_decimal;
                else
                    return null;
            }

            int n_int32;
            if (Int32.TryParse(inStr, out n_int32))
                return n_int32;



            long n_int64;
            if (Int64.TryParse(inStr, out n_int64))
                return n_int64;
            return null;
        }

        /// <summary>
        /// 把一个   [{"Name":"试试看\"我的\""},111,true]中的对象提取出来
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        private static ArrayList getList(String inStr) // 
        {

            ArrayList al = JSplit(inStr, '[', ',');

            for (int i = 0; i < al.Count; i++)
                al[i] = getToken((String)al[i]);

            return al;

        }
        /// <summary>
        /// 把一个{"ColumnName":"Name","DataType":"string"}对象中的obj提取出来
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        private static ArrayList getObj(String inStr) //
        {
            ArrayList al = JSplit(inStr, '{', ',');

            for (int i = 0; i < al.Count; i++)
                al[i] = getKV((String)al[i]);

            return al;
        }

        /// <summary>
        /// 将一个"ColumnName":"Name"类型的KV切出来
        /// 将Key 和Value切开取出
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        private static ArrayList getKV(String inStr)  //
        {
            inStr = inStr.TrimStart();
            char[] jstr = inStr.ToCharArray();
            if (jstr[0] != '"')
                return null; //应该有个exception

            ArrayList al = new ArrayList();

            int p = 1;

            while (p < jstr.Length)
                if (jstr[p] == '"' && !isEscaped(jstr, p))
                    break;
                else
                    p++;

            if (p == jstr.Length)
                return null; //只有Key没Value

            al.Add(getString(inStr.Substring(0, p + 1)));   //获得Key

            al.Add(getToken(inStr.Substring(p + 2, jstr.Length - p - 2)));

            return al;
        }

        /// <summary>
        /// 反转义，将一个JSON格式的字符串转化回正常C#的字符串
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        private static String getString(String inStr)    //
        {
            if (inStr.Length < 2)
                return null;          //应该有个exception
            else if (inStr.Length == 2)
                return "";

            StringBuilder s = new StringBuilder();

            char[] jstr = inStr.Substring(1, inStr.Length - 2).ToCharArray();
            int p = 0;

            while (p < jstr.Length - 1)
            {
                if (jstr[p] == '\\')
                {
                    switch (jstr[p + 1])
                    {
                        case '"':
                        case '\'':
                        case '\\':
                        case '/':
                            s.Append(jstr[p + 1]);
                            break;
                        case 'n':
                            s.Append('\n');
                            break;
                        case 't':
                            s.Append('\t');
                            break;
                        case 'r':
                            s.Append('\r');
                            break;
                        case 'b':
                            s.Append('\b');
                            break;
                        case 'f':
                            s.Append('\f');
                            break;
                        case 'u':
                            s.Append(getJsonChar(jstr, p));
                            p += 4;
                            break;
                        default:
                            s.Append('?');
                            break;
                    }

                    p++;
                }
                else
                    s.Append(jstr[p]);

                p++;
            }

            if (p != jstr.Length)
                s.Append(jstr[p]);

            return s.ToString();
        }

        /// <summary>
        /// 从一个四位16位进制字符串得到一个unicode字符
        /// </summary>
        /// <param name="inStr">输入的字符串</param>
        /// <returns></returns>
        private static char getJsonChar(char[] inCharA, int char_p)
        {
            if (char_p + 6 > inCharA.Length)
                return '?';

            string s = new string(inCharA, char_p + 2, 4);

            char b2 = (char)short.Parse(s, global::System.Globalization.NumberStyles.HexNumber);

            return b2;
        }

        /// <summary>
        /// 将一个JSON字符串用点号,或帽号 :切开成ArrayList进行下一步处理。
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="LeftClosure"></param>
        /// <param name="sep"></param>
        /// <returns></returns>
        private static ArrayList JSplit(String inStr, char LeftClosure, char sep)
        {
            char[] jstr = inStr.ToCharArray();
            ArrayList al = new ArrayList();

            if (jstr.Length < 3)
                return al;

            Stack flag = new Stack();
            int p = 0;
            int start = 0;
            int parse_error = -1;

            while (p < jstr.Length - 1)
            {
                if (jstr[p] == LeftClosure)          //找到左匹配符
                {
                    flag.Push(rightClosure(jstr[p]));
                    start = ++p;
                    break;
                }
                else
                {
                    p++;
                }
            }

            while (p < jstr.Length)
            {
                if (flag.Count == 1) //在外围，寻找分隔符
                {
                    if (jstr[p] == sep)   //找分隔符，找不到就向前进
                    {
                        al.Add(inStr.Substring(start, p - start));

                        start = p + 1;

                    }
                    else if (isLeftClosure(jstr[p]))
                    {
                        flag.Push(rightClosure(jstr[p]));
                    }
                    else if (jstr[p] == (char)flag.Peek())
                    {
                        break; // 提前结束了
                    }
                }
                else if (flag.Count > 1)
                {
                    if ((char)flag.Peek() == '"')   //在双引号里，只寻找双引号
                    {
                        if (jstr[p] == '"' && !isEscaped(jstr, p))  //找到另一个匹配的双引号，就退出双引号
                        {
                            flag.Pop();
                        }
                    }
                    else
                    {
                        if (jstr[p] == (char)flag.Peek())
                        {
                            flag.Pop();
                        }
                        else if (isLeftClosure(jstr[p]))
                        {
                            flag.Push(rightClosure(jstr[p]));
                        }
                    }
                }
                else
                {
                    break;
                }

                p++;
            }

            al.Add(inStr.Substring(start, p - start));

            if (parse_error != -1)
                return null;

            return al;

        }

        private static bool isSpace(char c)  //判断一个字符是否属于空格，包括回车，\t等。
        {
            return (c == ' ' || c == '\n' || c == '\r' || c == '\t');
        }

        private static bool isEscaped(char[] jstr, int p)   //判断一个字符是否是escape过的，就是判断它之前的\是偶数还是奇数
        {
            int numBackSlash = 0;
            while (p-- > 0)
                if (jstr[p] == '\\')
                    numBackSlash++;
                else
                    break;

            if (numBackSlash % 2 == 1)
                return true;
            else
                return false;

        }

        private static bool isLeftClosure(char c)
        {
            return (c == '"' || c == '{' || c == '[');
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static char rightClosure(char c)
        {
            if (c == '"')
                return '"';
            if (c == '[')
                return ']';
            if (c == '{')
                return '}';

            return '\0';
        }



        /// <summary>
        /// 一个将DataTable转化为JSon字符串序列的函数
        /// </summary>
        /// <param name="dt">输入的DataTable</param>
        /// <returns>返回的Json字符串，直接发送给Javascript</returns>
        private static string DT2JSON(DataTable dt)
        {
            /*
             * {"TableName":"table1","Columns":[{"ColumnName":"Name","DataType":"string"}],"Rows":[{"Name":"试试看\"我的\""}]}]
             */


            StringBuilder JsonString = new StringBuilder();

            if (dt != null)// && dt.Rows.Count > 0)
            {
                String TableName;

                if (dt.TableName == null || dt.TableName.ToString().Trim() == "")
                    TableName = "Table1";
                else
                    TableName = dt.TableName.ToString();

                JsonString.Append("{");
                JsonString.Append("\"DataType\":");
                JsonString.Append("\"System.DataTable\",");

                JsonString.Append("\"TableName\":");
                JsonString.Append("\"" + TableName + "\"");
                JsonString.Append(",");

                JsonString.Append("\"Columns\":[");

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    JsonString.Append("{\"ColumnName\":\"" + dt.Columns[i].ColumnName.ToString() + "\",");
                    JsonString.Append("\"" + "DataType\":\"" + checkDTType(dt.Columns[i].DataType.ToString()) + "\"}");


                    if (i < dt.Columns.Count - 1)
                        JsonString.Append(',');
                }

                JsonString.Append("],");

                JsonString.Append("\"Rows\":[");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append('{');

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":");
                        JsonString.Append(Obj2JSON(dt.Rows[i][j]));


                        if (j < dt.Columns.Count - 1)
                            JsonString.Append(',');
                    }

                    if (i < dt.Rows.Count - 1)
                        JsonString.Append("},");
                    else
                        JsonString.Append('}');
                }

                JsonString.Append("]}");
            }

            return JsonString.ToString();
        }

        /// <summary>
        /// 日期转换成JSON模式的字符串
        /// </summary>
        /// <param name="dte"></param>
        /// <returns></returns>
        private static string DateTime2JSON(DateTime dte)     //将DateTime 转成JSON格式。
        {
            String dtResult = "";

            dtResult = "{\"DataType\":\"System.DateTime\",\"Value\":\"" +
                dte.ToUniversalTime().ToString("s") + "Z\"}";

            return dtResult;
        }

        /// <summary>
        /// 将对像转换为JSON模式的字符串
        /// </summary>
        /// <param name="jobj"></param>
        /// <returns></returns>
        private static string Obj2JSON(Object jobj)   //将某个对象转成json格式。
        {
            if (jobj == null)
                return "null";

            String s = jobj.GetType().ToString();

            if (s == "System.Int32")
                return ((Int32)jobj).ToString();

            if (s == "System.Int16")
                return ((Int16)jobj).ToString();

            if (s == "System.Int64")
                return ((Int64)jobj).ToString();

            if (s == "System.Double")
                return ((Double)jobj).ToString();

            if (s == "System.Decimal")
                return ((Decimal)jobj).ToString();

            if (s == "System.Byte")
                return ((Byte)jobj).ToString();

            if (s == "System.Guid")
                return "\"" + ((Guid)jobj).ToString() + "\"";

            if (s == "System.Boolean")
                return ((Boolean)jobj).ToString().ToLower();

            if (s == "System.DateTime")
                return DateTime2JSON((DateTime)jobj);

            if (s == "System.DBNull")
                return "null";

            if (s == "System.Data.DataTable")
                return DT2JSON((DataTable)jobj);

            if (s == "System.String")
                return String2JSON((String)jobj);

            return "\"????\"";

        }


        private static string checkDTType(string s) //DataTable中仅支持如下几种类型几种种类型的数据
        {
            switch (s)
            {
                case "System.Int32":
                case "System.Int64":
                case "System.Char":
                case "System.DateTime":
                case "System.Boolean":
                case "System.String":
                case "System.Double":
                case "System.Decimal":
                case "System.DBNull":
                case "System.Byte":
                case "System.Guid":
                    return s;
                default:
                    return null;
            }
        }

        private static string String2JSON(string s)  //把一个字符串转换为JSON格式，转义"/\n等字符。如果是null就返回"null"
        {

            if (s == null)
                return "null";

            if (s.Length == 0)
            {
                return "\"\"";
            }

            char c;
            int i;
            int len = s.Length;
            StringBuilder sb = new StringBuilder(len + 4);
            string t = "";


            for (i = 0; i < len; i += 1)
            {
                c = s[i];
                if ((c == '\\') || (c == '"') || (c == '/'))
                {
                    sb.Append('\\');
                    sb.Append(c);
                }
                else if (c == '\b')
                    sb.Append("\\b");
                else if (c == '\t')
                    sb.Append("\\t");
                else if (c == '\n')
                    sb.Append("\\n");
                else if (c == '\f')
                    sb.Append("\\f");
                else if (c == '\r')
                    sb.Append("\\r");
                else
                {
                    if (c < ' ')
                    {
                        //t = "000" + Integer.toHexString(c); 
                        string tmp = new string(c, 1);
                        try
                        {
                            t = "000" + int.Parse(tmp, System.Globalization.NumberStyles.HexNumber);
                        }
                        catch
                        {
                            //Letao.Common.Functions.ERP_ERROR(new Letao.Common.LTDB(), "错误的json字符:" + tmp,true);
                            sb.Append(c);
                            continue;
                        }

                        sb.Append("\\u" + t.Substring(t.Length - 4));
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
            }

            return "\"" + sb.ToString() + "\"";
        }


        public static Object JsonEnecodeForSina(String inStr)
        {
            return ConvertFromJsonForSina(getToken(inStr));
        }

        private static Object ConvertFromJsonForSina(Object jObj)  //初步被解析过的ArrayList或者Obj被送到这里来，解析为C#的对象，number，datetime，arraylist，等
        {
            if (jObj == null)
                return null;                                //null应该是出了什么问题了，格式错误等。

            String s = jObj.GetType().ToString();           //如果不是null，那就首先判断一下对象的类型


            if (s != "System.Collections.ArrayList")        //对象不是ArrayList，那就是number之类东西，直接返回。
                return jObj;



            ArrayList al = (ArrayList)jObj;                 //下来判断输入值是不是一个单个对象，目前仅支持System.DataTable和System.DateTime */

            if (al[0] != null && al[0].GetType().ToString() == "System.Collections.ArrayList")
            {
                if (((ArrayList)al[0])[0].GetType().ToString() == "System.String")
                {
                    if ((String)(((ArrayList)al[0])[0]) == "DataType")
                        if ((String)(((ArrayList)al[0])[1]) == "System.DataTable")
                            return al2DataTable(al);
                        else if ((String)(((ArrayList)al[0])[1]) == "System.DateTime")
                            return al2DateTime(al);
                }
            }
            Hashtable ht_return = new Hashtable();

            /* 输入值是一个列表，则逐个处理，逐个返回*/
            ArrayList tmpal, tmpal2;
            for (int i = 0; i < al.Count; i++)
            {
                if (al[i] == null)
                    continue;

                String sType = al[i].GetType().ToString();

                if (sType == "System.Collections.ArrayList")
                {
                    tmpal = (ArrayList)al[i];
                    if (tmpal[0].GetType().ToString() == "System.Collections.ArrayList")
                    {
                        tmpal2 = (ArrayList)tmpal[0];

                        if ((String)tmpal2[0] == "DataType")
                        {
                            if ((String)tmpal2[1] == "System.DataTable")
                                al[i] = al2DataTable(tmpal);
                            else if ((String)tmpal2[1] == "System.DateTime")
                                al[i] = al2DateTime(tmpal);
                        }
                    }
                    if (tmpal[0].GetType().ToString() == "System.String")
                    {
                        if (tmpal[1].GetType().ToString() == "System.String")
                        {
                            string str = Convert.ToString(tmpal[1]);
                            ht_return.Add(Convert.ToString(tmpal[0]), str);
                            al[i] = (str);
                        }
                        //if (tmpal[1].GetType().ToString() == "System.Collections.ArrayList")
                        //{
                        //    ArrayList a3 = (ArrayList)tmpal[1];
                        //    al[i] = a3;
                        //}
                    }
                }
            }

            return ht_return;
        }
    }
}
