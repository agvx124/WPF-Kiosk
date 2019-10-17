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
        private Seat seat;
        public int SeatName
        {
            get => _seatName;
            set
            {
                _seatName = value;
                // set을 했을시 컨트롤에 추가
                seat = App.SeatData.listSeat[_seatName-1];
                //Console.WriteLine("table: " + value);
                //foreach (Food item in seat.FoodList)
                //{
                //    Console.WriteLine("item.Name: " + item.Name);
                //    Console.WriteLine("item.Count: " + item.Count);
                //}

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
            Console.WriteLine(lvFood.SelectedItems[0] as Food);
            Food food = lvFood.SelectedItems[0] as Food;
            AddSelectedImage(food);
            AddSelectedMenu(food);
            SetLvOrderItem();
        }

        public void SetLvOrderItem()
        {
            lvOrder.ItemsSource = null;
            tbTotalPrice.Text = "0";

            tbTotalPrice.Text = getTotalPrice().ToString();
            lvOrder.ItemsSource = seat.FoodList;
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
            //List<Food> foodList = seat.FoodList;
            if (seat.FoodList.Exists(x => x.Name == food.Name))
            {
                seat.FoodList.Find(x => x.Name == food.Name).Count++;
            }
            else
            {
                Food orderFood = new Food();
                orderFood.Name = food.Name;
                orderFood.ImagePath = food.ImagePath;
                orderFood.Price = food.Price;
                orderFood.Category = food.Category;
                orderFood.Count = 1;
                seat.FoodList.Add(orderFood);
            }
            //orderLogData는 결제 할때 orderFood, count를 가져오는 식으로 만들기
            tbTotalPrice.Text = getTotalPrice().ToString();
            //App.OrderLogData.Find(x => x.food.Name == food.Name).Count++;
        }

        private int getTotalPrice()
        {
            int total = 0;
            foreach (Food food in seat.FoodList)
            {
                total += food.Count * food.Price;
            }

            return total;
        }

        private void BtnPayment_Click(object sender, RoutedEventArgs e)
        {
            if (seat.FoodList.Count == 0)
            {
                MessageBox.Show("결제할 내용이 없습니다.", "빽다방");
                return;
            }

            String payFoodList = "";
            String payment = "";
            foreach (Food food in seat.FoodList)
            {
                payFoodList += food.Name + " x" + food.Count.ToString() + "\n";
            }
            if (rbCash.IsChecked == true)
            {
                payment = "현금";
            }
            else
            {
                payment = "카드";
            }

            if (MessageBox.Show("결제 메뉴\n" + payFoodList + "\n결제 방식\n" + payment +"\n\n총 결제 금액 - " + getTotalPrice().ToString() + "원", "빽다방", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (Food food in seat.FoodList)
                {
                    App.OrderLogData.Find(x => x.food.Name == food.Name).Count += food.Count;
                }
                MessageBox.Show("결제 성공하셨습니다!", "빽다방");
                seat.FoodList.Clear();
                this.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("결제를 취소하였습니다", "빽다방");
            }
        }

        private void ImgPlus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Food food = lvFood.SelectedItems[0] as Food;

                if (seat.FoodList.Exists(x => x.Name == food.Name))
                {
                    seat.FoodList.Find(x => x.Name == food.Name).Count++;
                }
                else
                {
                    Food orderFood = new Food();
                    orderFood.Name = food.Name;
                    orderFood.ImagePath = food.ImagePath;
                    orderFood.Price = food.Price;
                    orderFood.Category = food.Category;
                    orderFood.Count = 1;
                    seat.FoodList.Add(orderFood);
                }
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("선택된 아이템이 없습니다.");
            }
            finally
            {
                tbTotalPrice.Text = getTotalPrice().ToString();
                SetLvOrderItem();
            }
        }

        private void ImgMinus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Food food = (lvFood.SelectedItem as Food);
            //food.Count--;
            try
            {
                Food food = lvFood.SelectedItems[0] as Food;
               
                if (seat.FoodList.Find(x => x.Name == food.Name).Count != 0)
                {
                    seat.FoodList.Find(x => x.Name == food.Name).Count--;
                }
                else
                {
                    MessageBox.Show("0미만으로 수량을 줄일 수 없습니다.");
                }
            } catch (NullReferenceException ex)
            {
                MessageBox.Show("프로그램 오류");
            }
            finally
            {
                tbTotalPrice.Text = getTotalPrice().ToString();
                SetLvOrderItem();
            }
            
        }
    }
}
