using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingTrain.Domain.Common;

namespace BookingTrain.Domain.Entities
{
    public class Seat : BaseEntity
    {
        public int TrainId { get; set; }
        public string SeatNumber { get; set; }      // C1-S1
        public int CoachNumber { get; set; }        // Toa số mấy
        public int SeatTypeId { get; set; }

        public Train Train { get; set; }
        public SeatType SeatType { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}

