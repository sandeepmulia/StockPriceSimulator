if (!String.prototype.format) {
    String.prototype.format = function (o) {
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
    var stockTickerTable = $('#stockTickerGrid');
    var stockTickerGridBody = stockTickerTable.find('tbody');

    function formatStock(stock) {
        return $.extend(stock, {
            Bid: stock.Bid.toFixed(2),
            Ask: stock.Ask.toFixed(2),
            Symbol: stock.Symbol,
            Last: stock.Last.toFixed(2),
            Close: stock.Close.toFixed(2)
        });
    }

    $("#refreshBtn").click(function () {
        console.log($('#textInput').val());
        stockTickerHubProxy.invoke('subscribeMktDataForSpecifiedTickers', $('#textInput').val()).done(function (stocks) {
            stockTickerGridBody.empty(); // Ensure grid is reset 
            $.each(stocks, function () {
                var stock = formatStock(this);
                stockTickerGridBody.append(rowTemplate.format(stock));
            });
        });
    });

    stockTickerHubProxy.on('UpdateTicker', function (stock) {
        var displayStock = formatStock(stock),
        $row = $(rowTemplate.format(displayStock));
        stockTickerGridBody.find('tr[data-symbol="' + stock.Symbol + '"]').replaceWith($row);
    });

    connection.start().done(function () {
        console.log("Connection established..." + connection.id);
        stockTickerHubProxy.invoke('getAllTickers').done(function () {
            console.log("Initialisation complete.")
        });
    });

    $('#textInput').bind('input', function () {
        stockTickerHubProxy.invoke('findSpecificTickers', $(this).val()).done(function (symbols) {
            console.log("symbols" + symbols);
            stockTickerGridBody.empty(); // Ensure grid is reset 
            $.each(symbols, function () {
                var stock = formatStock(this);
                stockTickerGridBody.append(rowTemplate.format(stock));
            });
        });
    });

});

    

