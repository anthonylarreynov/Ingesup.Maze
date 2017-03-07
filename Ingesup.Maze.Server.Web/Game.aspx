<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Game.aspx.cs" Inherits="Ingesup.Maze.Server.Web.Game" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="ContentHead" runat="server">
    <link rel="stylesheet" type="text/css" href="./Resources/css/Game.css" />

    <script type="text/javascript" src="./Game.js"></script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentBody" runat="server">
    <asp:Panel ID="PnlGame" class="clearfix" runat="server">
        <div id="PnlLeft" class="panel" runat="server">
            <div class="logo">
                <a href="<%= ResolveUrl("~/Default.aspx?link=1") %>" target="_blank"><img src="<%= ResolveUrl("~/Resources/img/ingesup-logo.png") %>" alt="Ingesup" /></a>
            </div>
            <div class="players">
                <table id="TblPlayers" cellpadding="0" cellspacing="0" runat="server">
                    <thead>
                        <tr>
                            <th class="th-player">Player</th>
                            <th class="th-x">X</th>
                            <th class="th-y">Y</th>
                            <th class="th-nb-move"># Move</th>
                            <th class="th-time">Time</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                </table>
            </div>
        </div>
        <div class="maze">
            <div id="PnlMazeContent" class="content" runat="server"></div>
        </div>
    </asp:Panel>
    <div id="PnlError" class="error" style="display:none" runat="server"></div>
    <script type="text/javascript">
        var game = new Game({
            controls: {
                PnlGame: '<%= PnlGame.ClientID %>',
                PnlLeft: '<%= PnlLeft.ClientID %>',
                TblPlayers: '<%= TblPlayers.ClientID %>',
                PnlMazeContent: '<%= PnlMazeContent.ClientID %>',
                PnlError: '<%= PnlError.ClientID %>'
            },
            settings: {
                refreshInterval: <%= Ingesup.Maze.Server.Web.Configuration.Instance.MazeRefreshInterval %>,
                gameKey: '<%= GameKey %>',
                movePlayerMinInterval: <%= MovePlayerMinInterval %>,
                mazeWidth: <%= MazeWidth %>,
                mazeHeight: <%= MazeHeight %>,
                playerKey: '<%= PlayerKey %>',
                url: '<%= ResolveUrl("~/api/Game") %>'
            }
        });
    </script>
</asp:Content>
