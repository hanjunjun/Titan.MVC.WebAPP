<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorAll.aspx.cs" Inherits="Titan.WebMVC.ErrorAll" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>页面错误</title>
    <!-- Favicon-->
    <link rel="icon" href="/favicon.ico" type="image/x-icon">

    <link href="~/css/google_fonts/googleFonts.css" rel="stylesheet" type="text/css">
    <link href="~/css/google_fonts/googleIcon.css" rel="stylesheet" type="text/css">

    <!-- Bootstrap Core Css -->
    <link href="/plugins/bootstrap/css/bootstrap.css" rel="stylesheet">

    <!-- Waves Effect Css -->
    <link href="/plugins/node-waves/waves.css" rel="stylesheet" />

    <!-- Custom Css -->
    <link href="/css/style.css" rel="stylesheet">
    <script language="javascript" type="text/javascript">
        function CheckError_onclick() {
            var chk = document.getElementById("CheckError");
            var divError = document.getElementById("errorMsg");
            if (chk.checked) {
                divError.style.display = "inline";
            }
            else {
                divError.style.display = "none";
            }
        }
    </script>
</head>
<Div class="four-zero-four">
    <div class="four-zero-four-container">
        <div class="error-code">Error</div>
        <div class="error-message">我勒个去，页面炸了!</div>
        <div class="button-place">
            <a href="/Login/IndexView" class="btn btn-default btn-lg waves-effect">回到主页</a>
        </div>
        <div id="errorMsg" style="margin-top: 15px;text-align: center; /*display: none*/" runat="server">
            <asp:Label ID="ErrorHtml" runat="server" Text="Label"></asp:Label><br />
        </div>
    </div>
</Div>
</html>
