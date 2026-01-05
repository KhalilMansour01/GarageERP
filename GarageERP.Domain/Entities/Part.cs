using System;
using System.Collections.Generic;
using System.Text;

namespace GarageERP.Domain.Entities;

public class Part
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal PriceSingle { get; set; }
    public decimal PriceBulk { get; set; }
    public int Inventory { get; set; }
    public decimal BuyPrice { get; set; }
    public int SupplierId { get; set; }

    public Supplier? Supplier { get; set; }
    public List<PartsUsed> PartsUsed { get; set; } = new();
}
