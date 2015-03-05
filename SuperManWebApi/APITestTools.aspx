<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="APITestTools.aspx.cs" Inherits="SuperManWebApi.APITestTools" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        API地址:<asp:TextBox ID="TextBox1" runat="server" Width="529px" Height="90px"></asp:TextBox>
        <br />
        参数:<asp:TextBox ID="TextBox2" Rows="50" runat="server" Height="170px" Width="560px"></asp:TextBox>
        <br />
        <asp:RadioButtonList ID="RadioButtonList1" runat="server">
            <asp:ListItem Value="1" Selected="True">Get</asp:ListItem>
            <asp:ListItem Value="2">Post</asp:ListItem>
        </asp:RadioButtonList>
        <br/> 
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="提交" />
    
    </div>
        <p><%=Results %></p>
    </form>
</body>
</html>
