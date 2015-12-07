using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GMailLabelCleanup.Data.Models.Identity;

namespace GMailLabelCleanup.Data.Models.Filters
{
    [Table("Filters")]
    public class Filter
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FilterId { get; set; }

        [MaxLength(256)]
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [MaxLength(25)]
        public string ImportId { get; set; }

        [StringLength(500, MinimumLength = 1)]
        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime DateCreatedUtc { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public IList<FilterProperty> FilterProperties { get; set; }

    }
}