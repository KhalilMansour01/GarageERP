using System;
using System.Collections.Generic;
using System.Text;

namespace GarageERP.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public int Type { get; set; } // e.g., regular, VIP
    public string Name { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Email { get; set; } = "";
    public decimal Balance { get; set; }

    public List<Vehicle> Vehicles { get; set; } = new();
}