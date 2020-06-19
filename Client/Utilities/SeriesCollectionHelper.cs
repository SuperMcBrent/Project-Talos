using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utilities {
    class SeriesCollectionHelper {
        public static (SeriesCollection, string[], Func<double, string>) TransactionsChart(List<int> items, DateTime start, TimeSpan dur, TimeSpan interval) {
            SeriesCollection SeriesCollection = new SeriesCollection{
                new LineSeries {
                    Title = "#Transactions over time",
                    Values = new ChartValues<int>(items),
                    PointGeometry = null
                }
            };

            string[] Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            Func<double, string> YFormatter = value => value.ToString("C");

            throw new NotImplementedException();
        }
    }
}
