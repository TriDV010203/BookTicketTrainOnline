using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingTrain.Domain.Common;

namespace BookingTrain.Domain.Entities
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
