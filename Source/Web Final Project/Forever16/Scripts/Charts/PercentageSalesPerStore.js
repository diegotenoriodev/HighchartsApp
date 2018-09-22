angular
    .module('Charts', [])
    .controller('PercentageSalesPerStore', function ($scope, $http) {
        $scope.period = new Forever16.Core.Period(new Date(2010, 1, 1), new Date());
        $scope.api = null;

        $scope.getAPI = function () {
            if ($scope.api == null) {
                $scope.api = new Forever16.Core.API($http);
            }

            return $scope.api;
        };

        $scope.init = function () {
        }

        $scope.refresh = function () {
            $scope.error = "";

            if ($scope.period.begin > $scope.period.end) {
                $scope.error = "Begin should be lower or equal to end!";
                return;
            }

            $scope.getAPI()
                .getPercentageSalesPerStore($scope.period, $scope.reloadChart)
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
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Percentage of Sales per Store (from  '
                    + $scope.getBeginFormated() + ' to '
                    + $scope.getEndFormated() + ')'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },

                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        }
                    }
                },

                series: [{
                    name: 'Stores',
                    colorByPoint: true,
                    data: obj.data
                }]

            })
        };

        $scope.init();
    });