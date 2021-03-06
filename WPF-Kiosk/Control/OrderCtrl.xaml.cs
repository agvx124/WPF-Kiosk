﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WPF_Kiosk.Network;

namespace WPF_Kiosk.Control
{
    /// <summary>
    /// Interaction logic for MenuCtrl.xaml
    /// </summary>
    public partial class OrderCtrl : UserControl
    {
        public delegate void backHandler();
        public event backHandler OnBack;

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
                seat = App.SeatData.listSeat[_seatName - 1];

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

            lvFood.ItemsSource = App.FoodData.listFood;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (OnBack != null)
            {
                OnBack();
            }
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
            Food food = lvFood.SelectedItem as Food;
            AddSelectedImage(food);
            AddSelectedMenu(food);
            setOrderTimeToNow();
            SetLvOrderItem();
        }

        private void setOrderTimeToNow()
        {
            App.SeatData.orderTime = DateTime.Now.ToString("yyyy.MM.dd-HH:mm:ss");
            orderTime.Text = "최근 주문 시간: " + App.SeatData.orderTime;
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

        private String getPayFoodList()
        {
            string result = "";
            foreach (Food food in seat.FoodList)
            {
                Console.WriteLine(food.Name);
                result += food.Name + " x" + food.Count.ToString() + "\n";
            }
            return result;
        }

        private String checkPayment(bool isChecked)
        {
            if (isChecked)
            {
                return "현금";
            }
            return "카드";
        }

        private void BtnPayment_Click(object sender, RoutedEventArgs e)
        {
            if (seat.FoodList.Count == 0)
            {
                MessageBox.Show("결제할 내용이 없습니다.", "빽다방");
                return;
            }

            // 메뉴 출력
            String payFoodList = "";
            String payment = "";

            payFoodList = getPayFoodList();

            // 현금/카드 라디오 버튼 체크 확인

            payment = checkPayment(rbCash.IsChecked?? true);

            // 메세지 박스 결제 메뉴, 결제 방식, 결제 금액 출력 후 YES일시 true NO일시 false
            if (MessageBox.Show("결제 메뉴\n" + payFoodList + "\n결제 방식\n" + payment + "\n\n총 결제 금액 - " + getTotalPrice().ToString() + "원", "빽다방", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // 통계 위한 orderlog 데이터 추가
                foreach (Food food in seat.FoodList)
                {
                    App.OrderLogData.Find(x => x.food.Name == food.Name).Count += food.Count;
                }
                var id = App.LogedID;

                if (AsynchronousClient.isLogin == false)
                {
                    MessageBox.Show("서버가 연결되어 있지 않습니다.");
                    return;
                }
                AsynchronousClient.SendMessage("@" + id + "#" + getTotalPrice().ToString() + "원이 결제되었습니다.");
                MessageBox.Show("결제 성공하셨습니다!", "빽다방");
                // 결제 성공시 메뉴 clear, 메인화면으로 돌아가기
                seat.FoodList.Clear();

                ICollectionView view = CollectionViewSource.GetDefaultView(seat.FoodList);
                view.Refresh();


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
                Food food = setFoodbyList();

                AddSelectedMenu(food);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("선택된 아이템이 없습니다.", "빽다방");
            }
            finally
            {
                SetLvOrderItem();
            }
        }

        private void ImgMinus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Food food = setFoodbyList();


                if (seat.FoodList.Find(x => x.Name == food.Name).Count > 1)
                {
                    seat.FoodList.Find(x => x.Name == food.Name).Count--;
                }

                //상품개수가 0개일 때 리스트가 삭제됨 (어떠한 오류로 0보다 작을 때 사라진다..)
                //우선 Count의 값이 1이면 삭제하도록 짜둠
                else if (seat.FoodList.Find(x => x.Name == food.Name).Count == 1)
                {
                    int i = seat.FoodList.FindIndex(x => x.Name == food.Name);
                    seat.FoodList.RemoveAt(i);
                }

            }
            catch (NullReferenceException)
            {
                MessageBox.Show("음식이 선택되지 않았습니다.");
            }
            finally
            {
                SetLvOrderItem();
            }

        }

        //전체 메뉴 삭제 기능
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("정말 테이블의 주문 내용을 전부 삭제하시겠습니까?", "빽다방", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                seat.FoodList.Clear();

                SetLvOrderItem();

                MessageBox.Show("삭제되었습니다.", "빽다방");
            }
            else
            {
                MessageBox.Show("취소되었습니다.", "빽다방");
            }
        }

        private void btnCancelByMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Food food = setFoodbyList();

                int i = seat.FoodList.FindIndex(x => x.Name == food.Name);
                seat.FoodList.RemoveAt(i);
            }

            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("음식을 선택하지 않았습니다", "빽다방");
            }
            finally
            {
                SetLvOrderItem();
            }
        }

        // 리스트에 따른 Food 리턴해주는 함수
        // 선택되는 곳이 lvFood일수도 lvOrder일수도 있어서
        private Food setFoodbyList()
        {
            Food food = lvOrder.SelectedItem as Food;

            if (food == null)
            {
                food = lvFood.SelectedItem as Food;
            }

            return food;
        }
    }
}
