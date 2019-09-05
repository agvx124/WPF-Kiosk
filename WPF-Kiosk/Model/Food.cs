using System;
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
        public String Name;

        // 각 음식의 가격
        public int Price;

        // 각 음식의 카운트
        public int Count;

        // 카테고리
        public Category Category;
    }
}