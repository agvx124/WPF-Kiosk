using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Kiosk.Model;

namespace WPF_Kiosk.Control
{
    /// <summary>
    /// Interaction logic for StatCtrl.xaml
    /// </summary>
    public partial class StatCtrl : UserControl
    {

        public Func<ChartPoint, string> PointLabel { get; set; }

        public SeriesCollection SeriesCollection { get; set; }

        public StatCtrl()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection();
            foreach (OrderLog orderLog in App.OrderLogData)
            {
                PieSeries newItem = new PieSeries();
                newItem.Title = orderLog.food.Name;
                newItem.Values = new ChartValues<int>() { orderLog.Count };

                SeriesCollection.Add(newItem);
            }


            PointLabel = chartPoint =>
               string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            DataContext = this;            
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed; 
        }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }

        //private void setMenuChart()
        //{
        //    string[] arr = new string[] { "바나나우유", "딸기우유", "초코우유", "수박우유" };
        //    pie.Title = "바나나우유";

        //    PointLabel = chartPoint =>
        //    string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

        //    PointName = arr[0];

        //    DataContext = this;
        //}

        //private void AddMenuChartItem()
        //{
        //   lvcMenu.Series = new SeriesCollection
        //   {
        //       foreach ()
        //   }
        //}
    }
}
