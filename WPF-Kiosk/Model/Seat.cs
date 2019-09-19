using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Kiosk.Model
{
    public class Seat
    {
        public int Id { get; set; }

        public List<Food> FoodList { get; set; }
    }
}
