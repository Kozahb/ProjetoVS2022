using SalesWebMvc.Models.Enums;
using System;
using SalesWebMvc.Models.Enums;

namespace SalesWebMvc.Models
{
    public class SalesRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Amout { get; set; }
        public SalesStatus Status { get; set; }
        public Seller Saller { get; set; }

        public SalesRecord() { 
        }

        public SalesRecord(int id, DateTime date, double amout, SalesStatus status, Seller saller)
        {
            Id = id;
            Date = date;
            Amout = amout;
            Status = status;
            Saller = saller;
        }
    }



}
