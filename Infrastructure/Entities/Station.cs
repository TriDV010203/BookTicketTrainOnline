using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingTrain.Domain.Common;

namespace BookingTrain.Domain.Entities
{
    public class Station : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }
}
