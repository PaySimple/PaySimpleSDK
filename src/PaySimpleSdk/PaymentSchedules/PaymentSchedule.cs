using System;
using PaySimpleSdk.Helpers;

namespace PaySimpleSdk.PaymentSchedules
{
    public class PaymentSchedule
    {
        public int Id { get; set; }
        public PaymentScheduleType PaymentScheduleType { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public int CustomerId { get; set; }
        public ScheduleStatus ScheduleStatus { get; set; }
        public DateTime? NextPaymentDate { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
