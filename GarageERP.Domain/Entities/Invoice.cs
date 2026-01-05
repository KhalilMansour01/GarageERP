using System;
using System.Collections.Generic;
using System.Text;

namespace GarageERP.Domain.Entities;

public class Invoice
{
    public int Id { get; set; }
    public DateTime DateIssued { get; set; }
    public decimal NetPrice { get; set; }
    public int Discount { get; set; }
    public decimal FinalPrice { get; set; }

    public List<Job> Jobs { get; set; } = new();
}