using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GMailLabelCleanup.Data.Models.Identity
{
    [Table("GoogleAuthData")]
    public class GoogleAuthData
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GoogleAuthDataId { get; set; }

        [MaxLength(256)]
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        [MaxLength(500)]
        public string Key { get; set; }

        [Required]
        [MaxLength(500)]
        public string Value { get; set; }

        [Required]
        [MaxLength(500)]
        public string Type { get; set; }

        public DateTime DateCreatedUtc { get; set; }

    }
}