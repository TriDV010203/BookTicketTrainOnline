using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingTrain.Domain.Common;
using BookingTrain.Domain.Enums;

namespace BookingTrain.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public string Method { get; set; }
        public Ticket Ticket { get; set; }
    }
}
