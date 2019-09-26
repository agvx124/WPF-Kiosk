using LiveCharts;
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

namespace WPF_Kiosk.Control
{
    /// <summary>
    /// Interaction logic for StatCtrl.xaml
    /// </summary>
    public partial class StatCtrl : UserControl
    {
        public StatCtrl()
        {
            InitializeComponent();

            //SeriesCollection = new SeriesCollection
            //{
            //    new ColumnS
            //}
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed; 
        }

        public SeriesCollection seriesColletion { get; set; }
    }
}
