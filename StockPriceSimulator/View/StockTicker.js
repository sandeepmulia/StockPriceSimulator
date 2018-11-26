if (!String.prototype.supplant) {
    String.prototype.supplant = function (o) {
        return this.replace(/{([^{}]*)}/g,
            function (a, b) {
                var r = o[b];
                return typeof r === 'string' || typeof r === 'number' ? r : a;
            }
        );
    };
}

$(function () {

    var connection = $.hubConnection();
    var stockTickerHubProxy = connection.createHubProxy('stocktickerHub');
    connection.logging = true;
    var rowTemplate = '<tr data-symbol="{Symbol}"><td>{Symbol}</td><td>{Bid}</td><td>{Ask}</td><td>{Last}</td><td>{Close}</td></tr>';
    var table = $('#stockTickerGrid');
    var stockTickerGridBody = table.find('tbody');

    function formatStock(stock) {
        return $.extend(stock, {
            Bid: stock.Bid.toFixed(2),
            Symbol: stock.Symbol,
        });
    }


    $("#refreshBtn").click(function () {
        console.log("Refresh button clicked");
        stockTickerHubProxy.invoke('getAllTickers').done(function (stocks) {
            $.each(stocks, function () {
                console.log(this.Symbol + ":" + this.Bid);
                var stock = formatStock(this);
                stockTickerGridBody.append(rowTemplate.supplant(stock));
            });
        });
    });

    stockTickerHubProxy.updateStockPrice = function (stock) {
        var displayStock = formatStock(stock),
            $row = $(rowTemplate.supplant(displayStock));

        stockTickerGridBody.find('tr[data-symbol=' + stock.Symbol + ']')
            .replaceWith($row);
    }

    function init() {
        stockTickerHubProxy.invoke('getAllTickers', function (stocks) {
            console.log("INIT...");
            $.each(stocks, function () {
                console.log(this.Symbol + ":" + this.Bid);
                var stock = formatStock(this);
                stockTickerGridBody.append(rowTemplate.supplant(stock));
            });
        });
    }

    connection.start().done(function () {
        console.log("Connection established..." + connection.id);
        init();
    });

});

    

