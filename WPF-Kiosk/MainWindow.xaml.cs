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
using System.Windows.Threading;
using WPF_Kiosk.Common;
using WPF_Kiosk.Control;
using WPF_Kiosk.Model;

namespace WPF_Kiosk
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            App.SeatData.Load();
            AddListSeatItems();
            SetTimer();
        }

        private void AddListSeatItems()
        {
            foreach(Seat seat in App.SeatData.listSeat)
            {
                SeatCtrl seatCtrl = new SeatCtrl();
                seatCtrl.SetSeat(seat);

                lvSeat.Items.Add(seatCtrl);
            }
        }

        private void Timer_tick(object sender, EventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => 
            {
                tbCurrentTime.Text = DateTime.Now.ToString();
            });            
        }

        private void SetTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_tick;
            timer.Start();
        }

        private void TbStats_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StatControl.Visibility = Visibility.Visible;
            StatControl.Load();
        }

        private void LvSeat_Selected(object sender, RoutedEventArgs e)
        {
            if (lvSeat.SelectedItem == null)
            {
                return;
            }

            SeatCtrl seat = (lvSeat.SelectedItem as SeatCtrl);
            OrderControl.SeatName = seat.GetSeat();
            
            OrderControl.Visibility = Visibility.Visible;

            lvSeat.SelectedIndex = -1;
        }
    }
}
