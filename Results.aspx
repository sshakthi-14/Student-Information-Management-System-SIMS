<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Results.aspx.cs" Inherits="SoftwareEngineeringProject.Results" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Results</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>

<body>
<form runat="server">

<div class="container mt-4">

    <h2>My Academic Results</h2>

    <asp:Button ID="btnBack" runat="server" Text="← Back"
        CssClass="btn btn-secondary mb-3"
        OnClick="btnBack_Click" />

    <asp:GridView ID="gvResults" runat="server"
        CssClass="table table-striped" />

</div>

</form>
</body>
</html>
