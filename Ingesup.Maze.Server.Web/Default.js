/***************************************************************\
|*                                                             *|
|*                    Default Page Behavior                    *|
|*                                                             *|
\***************************************************************/

function Default(params) {
    this.initialize(params);
}

Default.prototype = {
    _params: null,
    _headerHeight: null,
    _bodyHeight: null,

    autoResize: function () {
        var windowHeight = $(window).height();

        var availableHeight = Math.round(windowHeight - this._headerHeight - 20);
        if (availableHeight >= this._bodyHeight) {
            $("#" + this._params.controls.PnlBody).removeClass("scroll").addClass("no-scroll").children(".tbl-body").height("auto");
        }
        else {
            $("#" + this._params.controls.PnlBody).removeClass("no-scroll").addClass("scroll").children(".tbl-body").height(availableHeight - 40);
        }
    },

    onReady: function () {
        this._headerHeight = Math.round($("#" + this._params.controls.PnlHeader).height());
        this._bodyHeight = Math.round($("#" + this._params.controls.PnlBody).height());
        
        $(window).bind("resize", $.proxy(this.autoResize, this));
        this.autoResize();
    },

    initialize: function (params) {
        this._params = params;

        $(document).ready($.proxy(this.onReady, this));
    }
}