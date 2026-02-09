using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace GarageERP.Domain.Entities;

public class Vehicle
{
    public int Id { get; set; }
    public int Number { get; set; } // registration number
    public int CustomerId { get; set; } // link to Customer
    public string Residence { get; set; } = "";
    public DateTime DateOfOwnership { get; set; }
    public DateTime DateOfOperation { get; set; }
    public string Brand { get; set; } = "";
    public int Year { get; set; }
    public string Model { get; set; } = "";
    public string UseType { get; set; } = "";
    public string EngineNumber { get; set; } = "";
    public string ChasisNumber { get; set; } = "";
    public int Odometer { get; set; }
    public string Color { get; set; } = "";
    public string Shape { get; set; } = "";
    public int TrunkChar { get; set; } // unclear, keeping as int
    public string EnginePower { get; set; } = "";
    public int Cylinders { get; set; }
    public int SeatsNum { get; set; }
    public int SeatsNextToDriver { get; set; }
    public int EmptyWeight { get; set; }
    public int CargoWeight { get; set; }
    public int TotalWeight { get; set; }

    // public Customer? Customer { get; set; }
    // public List<Contract> Contracts { get; set; } = new();
    // public List<Job> Jobs { get; set; } = new();
}
