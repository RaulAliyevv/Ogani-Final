﻿namespace OganiWebApp.Database.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<OrderProduct> OrderProducts { get; set; }
        public int Status { get; set; }
        public decimal SumTotalPrice { get; set; }
    }
}
