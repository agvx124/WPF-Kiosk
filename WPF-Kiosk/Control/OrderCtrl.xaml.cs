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
                //Seat seat = App.SeatData.listSeat[_seatName-1];

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
            App.SeatData.Load();
#if true
            lvFood.ItemsSource = App.FoodData.listFood;
            if (SeatName != 0)
            {
                lvOrder.ItemsSource = GetSeat().FoodList;
            }
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
                        SetLvFoodItem(eCategory.Coffee);
                        break;
                    }
                case 3:
                    {
                        SetLvFoodItem(eCategory.Drink);
                        break;
                    }
                case 4:
                    {
                        SetLvFoodItem(eCategory.Desert);
                        break;
                    }
                case 5:
                    {
                        SetLvFoodItem(eCategory.SignatureMenu);
                        break;
                    }
            }
        }

        private void SetLvFoodItem(eCategory category)
        {
            lvFood.ItemsSource = App.FoodData.listFood.FindAll(x => x.Category == category);
        }

        private void LvFood_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Food food = lvFood.SelectedItems[0] as Food;
            AddSelectedImage(food);
            AddSelectedMenu(food);
            SetLvOrderItem();
        }

        private void SetLvOrderItem()
        {
            foreach (Food item in GetSeat().FoodList)
            {
                Console.WriteLine("item.Name: " + item.Name);
                Console.WriteLine("item.Count: " + item.Count);
            }
            lvOrder.ItemsSource = null;
            lvOrder.ItemsSource = GetSeat().FoodList;
        }

        private Seat GetSeat()
        {
            //Console.WriteLine(SeatName);
            Seat seat = App.SeatData.listSeat.Find(x => x.Name == SeatName);

            if (seat == null)
            {
                Console.WriteLine("seat is null");
            }
            return seat;
        }

        private void AddSelectedImage(Food food)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(food.ImagePath, UriKind.RelativeOrAbsolute);
            bi.EndInit();
            selectedImage.Source = bi;
        }

        //seat.foodList에 추가도 하고 orderlogData count ++함.
        //food.count를 증가시키면 다른 쪽에서 food
        private void AddSelectedMenu(Food food)
        {
            List<Food> foodList = GetSeat().FoodList;
            if (foodList.Exists(x => x.Name == food.Name))
            {
                Food findFood = foodList.Find(x => x.Name == food.Name);
                findFood.Count++;
            }
            else
            {
                food.Count = 1;
                foodList.Add(food);
            }

            App.OrderLogData.Find(x => x.food.Name == food.Name).Count++;
        }
    }
}
