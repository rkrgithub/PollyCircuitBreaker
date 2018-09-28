using System;

namespace DataSvc.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}