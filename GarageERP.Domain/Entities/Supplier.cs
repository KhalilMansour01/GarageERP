using System;
using System.Collections.Generic;
using System.Text;

namespace GarageERP.Domain.Entities;

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";

    public List<Part> Parts { get; set; } = new();
}
