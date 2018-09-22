angular
    .module('Charts', [])
    .controller('SalesPerAgeAndGender', function ($scope, $http) {
        $scope.period = new Forever16.Core.Period(new Date(2010, 1, 1), new Date());
        $scope.api = null;
        $scope.error = "";

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
                .getSalesPerAgeAndGender($scope.period, $scope.reloadChart)
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
                    type: 'column',
                    height: '600px',
                    options3d: {
                        enabled: true,
                        alpha: 15,
                        beta: 15,
                        viewDistance: 25,
                        depth: 40
                    }
                },

                title: {
                    text: 'Quantity of Items sold in each store by gender and age group (from  '
                    + $scope.getBeginFormated() + ' to '
                    + $scope.getEndFormated() 
                },

                xAxis: {
                    categories: obj.categories,
                    labels: {
                        skew3d: true,
                        style: {
                            fontSize: '16px'
                        }
                    }
                },

                yAxis: {
                    allowDecimals: false,
                    min: 0,
                    title: {
                        text: 'Number of items sold',
                        skew3d: true
                    }
                },

                tooltip: {
                    headerFormat: '<b>{point.key}</b><br>',
                    pointFormat: '<span style="color:{series.color}">\u25CF</span> {series.name}: {point.y} / {point.stackTotal}'
                },

                plotOptions: {
                    column: {
                        stacking: 'normal',
                        depth: 40
                    }
                },

                series: obj.series
            });

            $scope.init();
        }
    });