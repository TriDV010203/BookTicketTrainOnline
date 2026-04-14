using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingTrain.Domain.Common;
using BookingTrain.Domain.Enums;

namespace BookingTrain.Domain.Entities
{
    public class Ticket : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ScheduleId { get; set; }
        public int SeatId { get; set; }
        public TicketStatus Status { get; set; }
        public User User { get; set; }
        public Schedule Schedule { get; set; }
        public Seat Seat { get; set; }
        public Payment? Payment { get; set; }
    }
}
