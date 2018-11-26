$(function () {

    var connection = $.hubConnection();
    var stockTickerHubProxy = connection.createHubProxy('stocktickerHub');
    connection.logging = true;
    var rowTemplate = '<tr data-symbol="{Symbol}"><td>{Symbol}</td><td>{Price}</td><td>{DayOpen}</td><td>{Direction} {Change}</td><td>{PercentChange}</td></tr>';
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
        stockTickerHubProxy.invoke('', $('#symbol').val(), $('#bid').val());
    });

    function init() {
        stockTickerHubProxy.invoke('getAllTickers', function (stocks) {
            console.log("INIT...");
            $.each(stocks, function () {
                consol.log(this.Symbol + ":" + this.Bid);
                var stock = formatStock(this);
                $stockTickerGridBody.append(rowTemplate.supplant(stock));
            });
        });
    }

    connection.start().done(function () {
        console.log("Connection established..." + connection.id);
        init();
    });

});

    

