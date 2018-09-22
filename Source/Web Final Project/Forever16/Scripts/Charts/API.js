var Forever16 =
    {
        Core: {

            API: function (http) {
                this.url = "/home/";
                this.http = http;

                this.getData = function(action, params, callBackFunction) {
                    this.http({
                        method: "POST",
                        url: this.url + action,
                        data: params,
                        dataType: 'json'
                    }).then(function (response) {
                        callBackFunction(response.data);
                    });
                };

                this.GetYears = function (callBackFunction) {
                    this.getData("GetYears", null, callBackFunction);
                };

                this.getSalesPerStore = function (params, callBackFunction) {
                    this.getData("GetSalesPerStore", params, data => callBackFunction($.parseJSON(data)));
                };

                this.getSalesPerProduct = function (params, callBackFunction) {
                    this.getData("GetSalesPerProduct", params, data => callBackFunction($.parseJSON(data)));
                };

                this.getPercentageSalesPerStore = function (params, callBackFunction) {
                    this.getData("GetPercentageSalesPerStore", params, data => callBackFunction($.parseJSON(data)));
                };

                this.getSalesPerAgeAndGender = function (params, callBackFunction) {
                    this.getData("GetSalesPerAgeAndGender", params, data => callBackFunction($.parseJSON(data)));
                };

                this.getAvailableProducts = function (callbackFunction) {
                    this.getData("GetAvailableProducts", null, data => callbackFunction($.parseJSON(data)));
                }
            },

            Period: function (begin, end) {
                this.begin = begin;
                this.end = end;
            },

            ListItem: function (text, value) {
                this.text = text;
                this.value = value;
            }
        }
    }
