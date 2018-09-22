angular
    .module('Charts', [])
    .controller('SalesPerStore', function ($scope, $http) {
        $scope.period = new Forever16.Core.Period(2010, 2010);
        $scope.availableYears = [];
        $scope.api = null;
        $scope.error = "";

        $scope.init = function () {
            $scope.getAPI().GetYears(years => {
                $scope.availableYears = years;

                $scope.period.begin = years[0];
                $scope.period.end = years[years.length - 1];
            });
        }

        $scope.getAPI = function () {
            if ($scope.api == null) {
                $scope.api = new Forever16.Core.API($http);
            }

            return $scope.api;
        };

        $scope.refresh = function () {
            $scope.error = "";

            if ($scope.period.begin > $scope.period.end) {
                $scope.error = "Begin should be lower or equal to end";
                return;
            }
            $scope.getAPI()
                .getSalesPerStore($scope.period, $scope.reloadChart)
        };

        $scope.reloadChart = function (obj) {
            Highcharts.chart('container', {

                title: {
                    text: 'Total of sales by store ' + $scope.period.begin + ' - ' + $scope.period.end
                },

                subtitle: {
                    text: ''
                },

                yAxis: {
                    title: {
                        text: 'Total ammount sold'
                    }
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle'
                },

                plotOptions: {
                    series: {
                        label: {
                            connectorAllowed: false
                        },
                        pointStart: obj.pointStart
                    }
                },

                series: obj.series,
                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 500
                        },
                        chartOptions: {
                            legend: {
                                layout: 'horizontal',
                                align: 'center',
                                verticalAlign: 'bottom'
                            }
                        }
                    }]
                }

            });
        }
        
        $scope.init();
    });