using System;
using System.Collections.Generic;
using System.Text;

namespace GarageERP.Domain.Entities;

public class Job
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public decimal NetCost { get; set; }
    public int ServiceId { get; set; }
    public DateTime Date { get; set; }

    public Vehicle? Vehicle { get; set; }
    public Service? Service { get; set; }
    public List<Invoice> Invoices { get; set; } = new();
}