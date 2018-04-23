<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="BatchEditDateRanges.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .container {
            width: 100%;
            margin: 5em auto;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div runat="server" class="container" id="PageContainer">
            <dx:ASPxFormLayout runat="server" ID="DateRangeFormLayout" Width="100%">
                <Items>
                    <dx:LayoutItem HorizontalAlign="Center" Caption="Start Date">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxDateEdit runat="server" ID="StartDate">
                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                        <RequiredField IsRequired="true"
                                            ErrorText="Start Date field is required" />
                                    </ValidationSettings>
                                </dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem HorizontalAlign="Center" Caption="End Date">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxDateEdit runat="server" ID="EndDate">
                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                        <RequiredField IsRequired="true"
                                            ErrorText="End Date field is required" />
                                    </ValidationSettings>
                                    <DateRangeSettings StartDateEditID="StartDate"></DateRangeSettings>
                                </dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem HorizontalAlign="Center" ShowCaption="False">
                        <Paddings PaddingTop="20px" />
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxHyperLink runat="server" ID="SubmitHyperLink"></dx:ASPxHyperLink>
                                <dx:ASPxButton runat="server" ID="SubmitButton" Text="Edit Date Range" 
                                    OnClick="SubmitButton_Click"></dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:ASPxFormLayout>




        </div>
    </form>
</body>
</html>