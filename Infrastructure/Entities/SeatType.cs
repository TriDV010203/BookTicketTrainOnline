using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingTrain.Domain.Common;

namespace BookingTrain.Domain.Entities
{
    public class SeatType : BaseEntity
    {
        public string Name { get; set; }           // VIP, Normal, Economy
        public string? Description { get; set; }

        public virtual ICollection<Seat> Seats { get; set; }
    }
}
