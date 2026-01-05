using System;
using System.Collections.Generic;
using System.Text;

namespace GarageERP.Domain.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Cost { get; set; }

        public List<Job> Jobs { get; set; } = new();
        public List<PartsUsed> PartsUsed { get; set; } = new();
    }
}
