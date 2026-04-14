using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingTrain.Domain.Common;
using BookingTrain.Domain.Enums;

namespace BookingTrain.Domain.Entities
{
    public class Seat : BaseEntity
    {
        public int Id { get; set; }
        public int TrainId { get; set; }
        public SeatType SeatType { get; set; }
        public string SeatNumber { get; set; }
        public Train Train { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
