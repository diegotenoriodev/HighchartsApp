angular
    .module('Charts', [])
    .controller('SalesPerProduct', function ($scope, $http) {
        $scope.period = new Forever16.Core.Period(new Date(2010, 1, 1), new Date(2017, 12, 31));
        $scope.availableProducts = [];
        $scope.selectedProductIds = [];
        $scope.api = null;
        $scope.error = "";

        $scope.isChecked = function (item) {
            return $scope.selectedProductIds.indexOf(item.value) >= 0;
        }

        $scope.updateCheck = function (item) {
            if ($scope.isChecked(item)) {
                $scope.selectedProductIds.splice($scope.selectedProductIds.indexOf(item.value), 1);
            } else {
                $scope.selectedProductIds.push(item.value);
            }
        }

        $scope.getTotalColumns = function () {
            var qtt = $scope.availableProducts.length / 5;
            var returnedItem = [];

            for (var i = 0; i <= qtt; i++) {
                returnedItem.push(i * 5);
            }

            return returnedItem;
        }

        $scope.getNext = function (initial, limit) {
            var returnedList = [];

            if ($scope.availableProducts.length > 0) {
                for (var i = initial; i < initial + limit; i++) {
                    if ($scope.availableProducts.length > i) {
                        returnedList.push($scope.availableProducts[i]);
                    }
                }
            }
            return returnedList;
        }

        $scope.getAPI = function () {
            if ($scope.api == null) {
                $scope.api = new Forever16.Core.API($http);
            }

            return $scope.api;
        };

        $scope.init = function () {
            $scope.getAPI().getAvailableProducts(products => {
                $scope.availableProducts = products;

                products.forEach(r => {
                   $scope.selectedProductIds.push(r.value);
                })
            });
        }

        $scope.refresh = function () {
            $scope.error = "";

            if ($scope.period.begin > $scope.period.end) {
                $scope.error = "Begin should be lower or equal to end!";
                return;
            } else if ($scope.selectedProductIds.length == 0) {
                $scope.error = "You must select at least one product!";
                return;
            }

            $scope.getAPI()
                .getSalesPerProduct({
                    products: $scope.selectedProductIds,
                    begin: $scope.period.begin,
                    end: $scope.period.end
                }, $scope.reloadChart)
        };

        $scope.getBeginFormated = function () {
            if ($scope.period.begin != null) {
                return moment($scope.period.begin).format('MMMM Do YYYY');
            } else {
                return "---";
            }
        };

        $scope.getEndFormated = function () {
            if ($scope.period.end != null) {
                return moment($scope.period.end).format('MMMM Do YYYY');
            } else {
                return "---";
            }
        };

        $scope.reloadChart = function (obj) {
            Highcharts.chart('container', {
                chart: {
                    type: 'bar',
                    height: '100%'
                },
                title: {
                    text: 'Quantity of products sold in each store (from  '
                    + $scope.getBeginFormated() + ' to '
                    + $scope.getEndFormated() 
                },
                xAxis: {
                    categories: obj.categories,
                    title: {
                        text: null
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Quantity of Items Sold',
                        align: 'high'
                    },
                    labels: {
                        overflow: 'justify'
                    }
                },
                tooltip: {
                    valueSuffix: ' units'
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true
                        }
                    },
                    series: {
                        pointWidth: 6
                    }
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'top',
                    x: -40,
                    y: 80,
                    floating: true,
                    borderWidth: 2,
                    backgroundColor: ((Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'),
                    shadow: true
                },
                credits: {
                    enabled: false
                },
                series: obj.series
            });
        };


        $scope.init();
    });