﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject.Entity
{
    public class Batch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BatchId { get; set; }
        public DateTime ImportDate { get; set; }
        public Guid AdminId { get; set; }
        public virtual List<BatchDetail> BatchDetails { get; set; }
    }
}
