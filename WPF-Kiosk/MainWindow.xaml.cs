using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
            OrderControl.OnBack += OrderCtrl_OnBack;
        }

        private void OrderCtrl_OnBack()
        {
            int seatNum = OrderControl.SeatName;
            
            Seat seat = App.SeatData.listSeat.Find(x => x.Name == seatNum);
            
            ICollectionView view = CollectionViewSource.GetDefaultView(seat.FoodList);
            view.Refresh();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SplashScreen splashScreen = new SplashScreen("Assets/logo.png");
            splashScreen.Show(true);
            Thread.Sleep(1000);

            App.SeatData.Load();

            AddListSeatItems();
            SetTimer();
            splashScreen.Show(false);
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
            tbCurrentTime.Text = DateTime.Now.ToString();
        }

        // 시간 표시 함수
        private void SetTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_tick;
            timer.Start();
        }

        // 통계 창 표시 함수
        private void TbStats_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StatControl.Visibility = Visibility.Visible;
            StatControl.Load();
        }

        // 선택했던 Seat 를 다시 선택하지 못하는 경우 때문에 만든 함수
        private void LvSeat_Selected(object sender, RoutedEventArgs e)
        {
            if (lvSeat.SelectedItem == null)
            {
                return;
            }

            SeatCtrl seat = (lvSeat.SelectedItem as SeatCtrl);
            OrderControl.SeatName = seat.GetSeat();

            OrderControl.SetLvOrderItem();

            OrderControl.Visibility = Visibility.Visible;

            lvSeat.SelectedIndex = -1;
        }

        // 프로그램 종료 함수
        private void TbExitProgram_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("프로그램을 종료하시겠습니까?", "빽다방", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

    }
}
