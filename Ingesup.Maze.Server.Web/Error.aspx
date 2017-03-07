<%@ Page Title="Ingesup - Maze - Technical Error" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="Ingesup.Maze.Server.Web.Error" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="ContentHead" runat="server">
    <link rel="stylesheet" type="text/css" href="./Resources/css/Error.css" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="error">
        <div class="header">
            <div class="logo"><a href="<%= ResolveUrl("~/Default.aspx") %>"><img src="<%= ResolveUrl("~/Resources/img/ingesup-logo.png") %>" alt="Ingesup" /></a></div>
        </div>
        <div class="body">
            <div class="error">A technical error has occured...</div>
        </div>
    </div>
</asp:Content>