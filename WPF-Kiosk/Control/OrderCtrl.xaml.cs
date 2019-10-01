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
using WPF_Kiosk.Common;
using WPF_Kiosk.Model;

namespace WPF_Kiosk.Control
{
    /// <summary>
    /// Interaction logic for MenuCtrl.xaml
    /// </summary>
    public partial class OrderCtrl : UserControl
    {
        private int _seatName;
        private string _selectedImage;
        public int SeatName
        {
            get => _seatName;
            set
            {
                _seatName = value;
                // set을 했을시 컨트롤에 추가
                Seat seat = App.SeatData.listSeat[_seatName-1];

                tbTableId.Text = _seatName.ToString() + "번 테이블";
            }
        }
        public string DisplayedImagePath
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
            }
        }

        public OrderCtrl()
        {
            InitializeComponent();
            this.Loaded += OrderCtrl_Loaded;

            MenuList.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(MenuList_MouseLeftButtonDown), true);
        }
        private void OrderCtrl_Loaded(object sender, RoutedEventArgs e)
        {
            App.FoodData.Load();
#if true
            lvFood.ItemsSource = App.FoodData.listFood;
#else

#endif
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void MenuList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int menuIndex = MenuList.SelectedIndex;
            switch (menuIndex)
            {
                case 1:
                    {
                        lvFood.ItemsSource = App.FoodData.listFood;
                        break;
                    }
                case 2:
                    {
                        setLvFoodItem(eCategory.Coffee);
                        break;
                    }
                case 3:
                    {
                        setLvFoodItem(eCategory.Drink);
                        break;
                    }
                case 4:
                    {
                        setLvFoodItem(eCategory.Desert);
                        break;
                    }
                case 5:
                    {
                        setLvFoodItem(eCategory.SignatureMenu);
                        break;
                    }
            }
        }

        private void setLvFoodItem(eCategory category)
        {
            lvFood.ItemsSource = App.FoodData.listFood.FindAll(x => x.Category == category);
        }

        private void LvFood_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Food food = lvFood.SelectedItems[0] as Food;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(food.ImagePath, UriKind.RelativeOrAbsolute);
            bi.EndInit();
            selectedImage.Source = bi;
        }
    }
}
