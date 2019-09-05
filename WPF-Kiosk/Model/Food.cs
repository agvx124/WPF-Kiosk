﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Kiosk.Common;

namespace WPF_Kiosk.Model
{
    class Food
    {
        // 음식명
        public String Name { get; set; }

        // 각 음식의 가격
        public int Price { get; set; }

        // 각 음식의 카운트
        public int Count { get; set; }

        // 카테고리
        public eCategory Category { get; set; }
    }
}