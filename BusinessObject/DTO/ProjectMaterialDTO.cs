﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class ProjectMaterialDTO
    {
        public Guid ProjectMaterialId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid MaterialId { get; set; }
        public string MaterialName { get; set; }
        public int Quantity { get; set; }
    }
}