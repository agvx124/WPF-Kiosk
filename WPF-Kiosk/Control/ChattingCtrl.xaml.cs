using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
using WPF_Kiosk.Network;

namespace WPF_Kiosk.Control
{
    /// <summary>
    /// Interaction logic for ChattingCtrl.xaml
    /// </summary>
    public partial class ChattingCtrl : UserControl
    {
        public ChattingCtrl()
        {
            InitializeComponent();
            AsynchronousClient.CreateSocket();
        }
    }
}
