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

        public void Load()
        {
            int[] categoryCount = new int[Enum.GetValues(typeof(Common.eCategory)).Length];

            amountMenuChart.Series = new SeriesCollection();
            salesMenuChart.Series = new SeriesCollection();
            amountCategoryChart.Series = new SeriesCollection();
            salesCategoryChart.Series = new SeriesCollection();

            App.OrderLogData.Where(x => x.food.Category == Common.eCategory.Coffee).Count();

            foreach (OrderLog orderLog in App.OrderLogData)
            {
                if (orderLog.Count != 0)
                {
                    PieSeries newItem = setPieSeriesItem(orderLog);

                    newItem.Values = new ChartValues<int>() { orderLog.Count };

                    amountMenuChart.Series.Add(newItem);
                }

                if (orderLog.Count != 0)
                {
                    PieSeries newItem = setPieSeriesItem(orderLog);

                    newItem.Values = new ChartValues<int>() { orderLog.Count * orderLog.food.Price };

                    salesMenuChart.Series.Add(newItem);
                }

                //switch (orderLog.food.Category)
                //{
                //    case Common.eCategory.Coffee:
                //        categoryCount[0] += orderLog.Count;
                //    case Common.eCategory.Desert:
                //        categoryCount[1] += orderLog.Count;
                //}

                //for (int i = 0; i < orderLog.Count; i++)
                //{
                //    if (orderLog.food.Category == Common.eCategory.Coffee)
                //    {
                //        categoryCount[0] += orderLog.food.Count;
                //    }
                //    else if (orderLog.food.Category == Common.eCategory.Desert)
                //    {
                //        categoryCount[1] += orderLog.food.Count;
                //    }
                //    else if (orderLog.food.Category == Common.eCategory.Drink)
                //    {
                //        categoryCount[2] += orderLog.food.Count;
                //    }
                //    else if (orderLog.food.Category == Common.eCategory.SignatureMenu)
                //    {
                //        categoryCount[3] += orderLog.food.Count;
                //    }
                //}
            }

            DataContext = this;
        }

        private PieSeries setPieSeriesItem(OrderLog orderLog)
        {
            PieSeries newItem = new PieSeries();
            newItem.Title = orderLog.food.Name;

            newItem.DataLabels = true;
            newItem.LabelPoint = PointLabel;
            newItem.Style = Resources["PieSeriesStyle"] as Style;

            return newItem;
        }
    }
}
