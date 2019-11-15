<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebForms._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <asp:Button ID="btnTest" runat="server" OnClick="btnTest_Click" Text="Show PageLoadCalledCount" />
    </div>

    <div class="row">
        <div class="col-md-4">
            <p>
                &nbsp;<asp:Label ID="lblTest" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>
            </p>
        </div>
        <div class="col-md-4">
        </div>
        <div class="col-md-4">
            <h2>&nbsp;</h2>
        </div>
    </div>

</asp:Content>
