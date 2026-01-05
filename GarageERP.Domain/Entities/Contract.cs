using System;
using System.Collections.Generic;
using System.Text;

namespace GarageERP.Domain.Entities;

public class Contract
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public int Discount { get; set; }
    public DateTime DateIssued { get; set; }
    public DateTime ExpDate { get; set; }

    public Vehicle? Vehicle { get; set; }
}