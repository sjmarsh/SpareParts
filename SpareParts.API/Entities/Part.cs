﻿using SpareParts.Shared.Models;

namespace SpareParts.API.Entities
{
    public class Part : EntityBase
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public double Weight { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PartCategory? Category { get; set; }
        public List<PartAttribute>? Attributes { get; set; }
    }
}
