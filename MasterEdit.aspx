<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/BaseTemplate.Master" AutoEventWireup="true" CodeBehind="MasterEdit.aspx.cs" Inherits="SHOP.MasterEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <asp:TextBox ID="TxtName" runat="server"></asp:TextBox>
    <asp:TextBox ID="TxtDescription" runat="server"></asp:TextBox>
    <asp:TextBox ID="TxtImage" runat="server"></asp:TextBox>
    <asp:DropDownList ID="DrpCategories" runat="server">
        <asp:ListItem></asp:ListItem>
    </asp:DropDownList>
    <br />
    <asp:Button ID="BtnUpdate" runat="server" Text="Update" OnClick="BtnUpdate_Click" />
    </asp:Content>
