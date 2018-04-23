<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditRange.aspx.cs" Inherits="BatchEditDateRanges.EditRange" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div runat="server" class="container" id="PageContainer">
            <h2 runat="server" id="PageHeader" style="text-align:center;"></h2>
            <dx:ASPxGridView runat="server" ID="Grid" KeyFieldName="ID" Width="100%"
                OnDataBinding="Grid_DataBinding" OnCustomUnboundColumnData="Grid_CustomUnboundColumnData"
                OnBatchUpdate="Grid_BatchUpdate">
                <SettingsEditing Mode="Batch"></SettingsEditing>
            </dx:ASPxGridView>
        </div>
    </form>
</body>
</html>
