﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.Models
{
    public class Supplier : User

    {
        [ForeignKey("ActivityCategory")]
        public int? ActivityCategoryId { get; set; }
        public ActivityCategory? ActivityCategory { get; set; }
        public ICollection<SupplierPaymentMethod> SupplierPaymentMethods { get; set; } = new List<SupplierPaymentMethod>();
        public ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Order> Orders{ get; set; } = new List<Order>();
    }
}
    