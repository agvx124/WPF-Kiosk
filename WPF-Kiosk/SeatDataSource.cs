using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Kiosk.Model;


namespace WPF_Kiosk
{
    public class SeatDataSource
    {
        bool isLoaded = false;
        public List<Seat> listSeat = null;

        public void Load()
        {
            if (isLoaded) return;

            listSeat = new List<Seat>()
            {
                new Seat() { Id = 1},
                new Seat() { Id = 2},
                new Seat() { Id = 3},
                new Seat() { Id = 4},
                new Seat() { Id = 5},
                new Seat() { Id = 6}
            };

            isLoaded = true;
        }

    }
}
