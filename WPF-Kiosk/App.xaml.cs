﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPF_Kiosk.Model;

namespace WPF_Kiosk
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        public static SeatDataSource SeatData = new SeatDataSource();

        public static FoodDataSource FoodData = new FoodDataSource();

        public static List<OrderLog> OrderLogData = new List<OrderLog>();
        
        public static string LogedID = null;
    }
}
