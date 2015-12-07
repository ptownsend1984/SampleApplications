using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GMailLabelCleanup.Data.Models.Filters
{
    [Table("FilterProperties")]
    public class FilterProperty
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FilterPropertyId { get; set; }

        [Required]
        public int FilterId { get; set; }
        [ForeignKey("FilterId")]
        public Filter Filter { get; set; }

        public bool IsIncluded { get; set; }

        [MaxLength(256)]
        [Required]
        public string Name { get; set; }

        [MaxLength(1000)]        
        public string Value { get; set; }

        [Required]
        public DateTime DateCreatedUtc { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

    }
}