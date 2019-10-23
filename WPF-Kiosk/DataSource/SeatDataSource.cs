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
        public string orderTime = "X";
        readonly int NUMBER_OF_TABLE = 6; 

        public void Load()
        {
            if (isLoaded) return;

            listSeat = new List<Seat>();
            for (int i = 0; i < NUMBER_OF_TABLE; i++)
            {
                Seat seat = new Seat();
                seat.Id = i;
                seat.Name = i + 1;

                listSeat.Add(seat);
            }

            isLoaded = true;
        }
    }
}
