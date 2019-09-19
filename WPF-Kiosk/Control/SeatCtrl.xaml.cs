﻿using System;
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
    /// Interaction logic for SeatCtrl.xaml
    /// </summary>
    public partial class SeatCtrl : UserControl
    {
        public SeatCtrl()
        {
            InitializeComponent();
        }


        public void SetSeat(int id)
        {
            tbId.Text = id.ToString();
        }

        public void SetSeat(Seat seat)
        {
            tbId.Text = seat.Id.ToString();
        }
    }
}