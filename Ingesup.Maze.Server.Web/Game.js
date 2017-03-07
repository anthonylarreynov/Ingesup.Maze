/************************************************************\
|*                                                          *|
|*                    Game Page Behavior                    *|
|*                                                          *|
\************************************************************/

function Game(params) {
    this.initialize(params);
}

Game.prototype = {
    _params: null,
    _playersTable: null,
    _mazeContainer: null,
    _cellSize: null,
    _refreshTimer: null,
    _lastUpdateDate: null,
    _waitKeyDown: false,

    getCellId: function (position) {
        return this._params.controls.PnlMazeContent + '-' + position.X + '-' + position.Y;
    },

    getCellClass: function (cell) {
        switch (cell.CellType) {
            case 0: return "empty";
            case 1: return "wall";
            case 2: return "start";
            case 3: return "end";
        }
    },

    getCellTop: function (cell) {
        return this._mazeContainer.offset().top + cell.Position.Y * this._cellSize;
    },

    getCellLeft: function (cell) {
        return this._mazeContainer.offset().left + cell.Position.X * this._cellSize;
    },

    getCellStyle: function (cell) {
        return 'top:' + this.getCellTop(cell) + 'px;left:' + this.getCellLeft(cell) + 'px;width:' + this._cellSize + 'px;height:' + this._cellSize + 'px';
    },

    showCell: function (cell) {
        var id = this.getCellId(cell.Position);
        if (this._mazeContainer.find("#" + id).length > 0) {
            return;
        }

        var html = '<div id="' + id + '" class="cell ' + this.getCellClass(cell) + '" style="' + this.getCellStyle(cell) + '"></div>';
        this._mazeContainer.append(html);

        if (cell.CellType == 3) {
            this._mazeContainer.find("#" + id).bind("click", $.proxy(this.onEndCellClick, this));
        }
    },

    showPlayer: function (player) {
        var row = this._playersTable.find('[id=' + player.Id + ']');
        if (row.length > 0) {
            row.addClass("refreshed");
            row.children(".td-x").text(player.CurrentPosition.X);
            row.children(".td-y").text(player.CurrentPosition.Y);
            row.children(".td-nb-move").text(player.NbMove);
            if (player.FinishTime) {
                row.children(".td-time").text(player.FinishTime);
            }
        }
        else {
            var html = '<tr id="' + player.Id + '" class="refreshed">'
                + '<td class="td-name">' + player.Name + '</td>'
                + '<td class="td-x">' + player.CurrentPosition.X + '</td>'
                + '<td class="td-y">' + player.CurrentPosition.Y + '</td>'
                + '<td class="td-nb-move">' + player.NbMove + '</td>'
                + '<td class="td-time">' + (player.FinishTime ? player.FinishTime : "&nbsp;") + '</td>'
                + '</tr>';

            this._playersTable.children("tbody").append(html);
        }

        for (var i in player.VisibleCells) {
            this.showCell(player.VisibleCells[i]);
        }

        this._mazeContainer.find('.player-' + player.Id).remove();
        var playerCell = this._mazeContainer.find("#" + this.getCellId(player.CurrentPosition));
        playerCell.append('<div class="player player-' + player.Id + '"></div>');
    },

    removePlayers: function () {
        this._playersTable.children("tbody").children("tr").each($.proxy(function (index, e) {
            if ($(e).hasClass("refreshed")) {
                $(e).removeClass("refreshed")
                return;
            }

            var id = $(e).prop("id");
            if (!id) {
                return;
            }

            this._mazeContainer.find('.player-' + id).remove();
            $(e).remove();
        }, this));
    },

    onGetGameSuccess: function (data) {
        this._lastUpdateDate = data.ServerDate;

        for (var p in data.Players) {
            this.showPlayer(data.Players[p]);
        }

        this.removePlayers();

        for (var v in data.VisitedCells) {
            this.showCell(data.VisitedCells[v]);
        }
    },

    onGetGameFailed: function (jqXHR, textStatus, err) {
        window.clearInterval(this._refreshTimer);
        $("#" + this._params.controls.PnlError).text("An error occurred while refreshing data: " + textStatus).show();
    },

    onMovePlayerSuccess: function (data) {
        this.showPlayer(data);
    },

    onGetSecretMessageSuccess: function (data) {
        if (!data) {
            return;
        }

        alert(data);
    },

    MovePlayer: function (direction) {
        var url = this._params.settings.url + '?key=' + this._params.settings.gameKey + '&player=' + this._params.settings.playerKey + '&direction=' + direction;
        $.getJSON(url).done($.proxy(this.onMovePlayerSuccess, this));
    },

    Refresh: function () {
        var url = this._params.settings.url + '?key=' + this._params.settings.gameKey + '&lastUpdateDate=' + this._lastUpdateDate;
        $.getJSON(url)
            .done($.proxy(this.onGetGameSuccess, this))
            .fail($.proxy(this.onGetGameFailed, this));
    },

    RunAutoRefresh: function () {
        if (this._params.settings.gameKey === "") {
            return;
        }

        this._refreshTimer = window.setInterval($.proxy(this.Refresh, this), this._params.settings.refreshInterval);
        this.Refresh();
    },

    onEndCellClick: function (e) {
        var url = this._params.settings.url + '?key=' + this._params.settings.gameKey;
        $.getJSON(url).done($.proxy(this.onGetSecretMessageSuccess, this));
    },

    onKeyDown: function (e) {
        if (this._waitKeyDown) {
            return;
        }

        this._waitKeyDown = true;
        setTimeout($.proxy(function () { this._waitKeyDown = false; }, this), this._params.settings.movePlayerMinInterval);

        var direction;
        switch (e.which) {
            case 37: direction = "Left"; break;
            case 38: direction = "Up"; break;
            case 39: direction = "Right"; break;
            case 40: direction = "Down"; break;
            default: return;
        }

        e.preventDefault();

        this.MovePlayer(direction);
    },

    onErrorClick: function (e) {
        $("#" + this._params.controls.PnlError).hide();
        this.RunAutoRefresh();
    },

    initializeMaze: function () {
        var cellWidth = Math.floor(($(window).width() - $("#" + this._params.controls.PnlLeft).width() - 100) / this._params.settings.mazeWidth);
        var cellHeight = Math.floor(($(window).height() - 10) / this._params.settings.mazeHeight);
        this._cellSize = Math.min(cellWidth, cellHeight);

        this._mazeContainer.width(this._cellSize * this._params.settings.mazeWidth);
        this._mazeContainer.height(this._cellSize * this._params.settings.mazeHeight);
    },

    onReady: function () {
        this._playersTable = $("#" + this._params.controls.TblPlayers);
        this._mazeContainer = $("#" + this._params.controls.PnlMazeContent);
        this.initializeMaze();

        if (this._params.settings.playerKey !== "") {
            $(document).keydown($.proxy(this.onKeyDown, this));
        }

        $("#" + this._params.controls.PnlError).bind("click", $.proxy(this.onErrorClick, this));
        this.RunAutoRefresh();
    },

    initialize: function (params) {
        this._params = params;

        $(document).ready($.proxy(this.onReady, this));
    }
}