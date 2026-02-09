using System;
using System.Collections.Generic;
using System.Text;

namespace GarageERP.Domain.Entities;

public class Contract
{
    public int Id { get; set; }

    public int VehicleId { get; set; }

    // Percentage discount (0–100)
    public int Discount { get; set; }

    public DateTime DateIssued { get; set; }

    public DateTime ExpDate { get; set; }

    // 1 = Active, 2 = Closed, 3 = Expired
    public int Status { get; set; }

    public string Description { get; set; } = "";

    // public Vehicle? Vehicle { get; set; }
}
