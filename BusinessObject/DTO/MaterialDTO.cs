using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class MaterialDTO
    {
        public Guid MaterialId { get; set; }
        public Guid MaterialTypeId { get; set; }
        public string Name { get; set; }
        public float MediumPrice { get; set; }
        public string MaterialTypeName { get; set; }
        public string UnitType { get; set; }
    }
}
