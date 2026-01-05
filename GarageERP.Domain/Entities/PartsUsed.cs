using System;
using System.Collections.Generic;
using System.Text;

namespace GarageERP.Domain.Entities;

public class PartsUsed
{
    public int Id { get; set; }
    public int PartId { get; set; }
    public int ServiceId { get; set; }

    public Part? Part { get; set; }
    public Service? Service { get; set; }
}
