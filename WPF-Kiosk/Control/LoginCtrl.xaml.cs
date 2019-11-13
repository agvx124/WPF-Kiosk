using Newtonsoft.Json.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using WPF_Kiosk.Network;

namespace WPF_Kiosk.Control
{
    public partial class LoginCtrl : UserControl
    {
        public LoginCtrl()
        {
            InitializeComponent();
        }

        // 로그인 버튼 클릭시 수행하는 이벤트
        // 개인 서버 (restful api)와 소켓 서버 통신 두개 한꺼번에 연결
        // 뭔가 부족 리팩토링 해야함
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            ApiNetwork apiNetwork = new ApiNetwork();
            AsynchronousClient.CreateSocket();

            JObject jObject = JObject.Parse(apiNetwork.PostLogin(tbId.Text, tbPassword.Password.ToString()));
            int status = Convert.ToInt32(jObject["status"].ToString());
            string message = jObject["message"].ToString();

            if (status == 200)
            {
                AsynchronousClient.Send("@" + tbId.Text);

                var data = jObject["data"];
                string name = data["name"].ToString();

                App.LogedID = tbId.Text;

                MessageBox.Show("안녕하세요! " + name + "님", "로그인 성공!");
                this.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show(message, "빽다방");
                return;
            }
        }
    }
}