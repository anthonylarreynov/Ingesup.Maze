<%@ Page Title="Ingesup - Maze - Homepage" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Ingesup.Maze.Server.Web.Default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="ContentHead" runat="server">
    <link rel="stylesheet" type="text/css" href="./Resources/css/Default.css" />

    <script type="text/javascript" src="./Default.js"></script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="default">
        <div id="PnlHeader" class="header" runat="server">
            <div class="logo"><a href="<%= Request.RawUrl %>"><img src="<%= ResolveUrl("~/Resources/img/ingesup-logo.png") %>" alt="Ingesup" /></a></div>
        </div>
        <div id="PnlBody" class="body" runat="server">
            <asp:Repeater ID="RptGames" OnItemDataBound="RptGames_ItemDataBound" runat="server">
                <HeaderTemplate>
                    <div class="tbl-head">
                        <table cellpadding="0" cellspacing="0">
                            <thead>
                                <tr>
                                    <th class="create-date">Create Date</th>
                                    <th class="key">Game</th>
                                    <th class="difficulty">Mode</th>
                                    <th class="created-by">Created By</th>
                                    <th class="nb-players"># Players</th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                    <div class="tbl-body">
                        <table cellspacing="0" cellpadding="0">
                            <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="<%# (Container.ItemIndex % 2 == 0) ? "odd" : "even" %>">
                        <td class="create-date"><asp:Literal ID="LitCreateDate" runat="server" /></td>
                        <td class="key"><asp:Literal ID="LitKey" runat="server" /><asp:HyperLink ID="HplKey" Target="_blank" runat="server" /></td>
                        <td class="difficulty"><asp:Literal ID="LitDifficulty" runat="server" /></td>
                        <td class="created-by"><asp:Literal ID="LitCreator" runat="server" /></td>
                        <td class="nb-players"><asp:Literal ID="LitPlayersCount" runat="server" /></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                            </tbody>
                        </table>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <script type="text/javascript">
        var defaultPage = new Default({
            controls: {
                PnlHeader: '<%= PnlHeader.ClientID %>',
                PnlBody: '<%= PnlBody.ClientID %>'
            }
        });
    </script>
</asp:Content>