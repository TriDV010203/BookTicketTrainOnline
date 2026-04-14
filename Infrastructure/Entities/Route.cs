using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingTrain.Domain.Common;

namespace BookingTrain.Domain.Entities
{
    public class Route : BaseEntity
    {
        public int Id { get; set; }
        public int FromStationId { get; set; }
        public int ToStationId { get; set; }
        public Station FromStation { get; set; }
        public Station ToStation { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
