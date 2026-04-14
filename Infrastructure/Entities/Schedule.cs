using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingTrain.Domain.Common;

namespace BookingTrain.Domain.Entities
{
    public class Schedule : BaseEntity
    {
        public int Id { get; set; }
        public int TrainId { get; set; }
        public int RouteId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public Train Train { get; set; }
        public Route Route { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
