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
            Dictionary<Common.eCategory, int> categoryCount = new Dictionary<Common.eCategory, int>();
            Dictionary<Common.eCategory, int> categorySales = new Dictionary<Common.eCategory, int>();

            amountMenuChart.Series = new SeriesCollection();
            salesMenuChart.Series = new SeriesCollection();
            amountCategoryChart.Series = new SeriesCollection();
            salesCategoryChart.Series = new SeriesCollection();

            // 메뉴별 통계
            foreach (OrderLog orderLog in App.OrderLogData)
            {
                if (orderLog.Count != 0)
                {
                    PieSeries amountMenuItem = setPieSeriesItem(orderLog);
                    PieSeries salesMenuItem = setPieSeriesItem(orderLog);

                    amountMenuItem.Values = new ChartValues<int>() { orderLog.Count };
                    salesMenuItem.Values = new ChartValues<int>() { orderLog.Count * orderLog.food.Price };

                    amountMenuChart.Series.Add(amountMenuItem);
                    salesMenuChart.Series.Add(salesMenuItem);
                }
            }

            // 카테고리별 데이터 구하기
            for (int i = 0; i < App.OrderLogData.Count; i++)
            {
                if (categoryCount.ContainsKey(App.OrderLogData[i].food.Category) == true)
                {
                    categoryCount[App.OrderLogData[i].food.Category] += App.OrderLogData[i].Count;
                    categorySales[App.OrderLogData[i].food.Category] += App.OrderLogData[i].Count * App.OrderLogData[i].food.Price;
                }
                else
                {
                    categoryCount.Add(App.OrderLogData[i].food.Category, App.OrderLogData[i].Count);
                    categorySales.Add(App.OrderLogData[i].food.Category, App.OrderLogData[i].Count * App.OrderLogData[i].food.Price);
                }
            }


            // 카테고리별 통계
            foreach (KeyValuePair<Common.eCategory, int> pair in categoryCount)
            {
                if (pair.Value != 0)
                {
                    PieSeries amountCategoryItem = setPieSeriesItem(pair);
                    PieSeries salesCategoryItem = setPieSeriesItem(pair);

                    amountCategoryItem.Values = new ChartValues<int>() { categoryCount[pair.Key] };
                    salesCategoryItem.Values = new ChartValues<int>() { categorySales[pair.Key] };

                    amountCategoryChart.Series.Add(amountCategoryItem);
                    salesCategoryChart.Series.Add(salesCategoryItem);
                }
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

        private PieSeries setPieSeriesItem(KeyValuePair<Common.eCategory, int> pair)
        {
            PieSeries newItem = new PieSeries();
            newItem.Title = pair.Key.ToString();

            newItem.DataLabels = true;
            newItem.LabelPoint = PointLabel;
            newItem.Style = Resources["PieSeriesStyle"] as Style;

            return newItem;
        }
    }
}
