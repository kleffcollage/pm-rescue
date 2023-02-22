using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using PropertyMataaz.Utilities.Constants;

namespace PropertyMataaz.Models.AppModels
{
    public class Code : BaseModel
    {
        [Required]
        [MaxLength(ModelConstants.MAX_LENGTH_50)]
        [Column(TypeName = "varchar")]
        public string CodeString { get; set; }
        [MaxLength(ModelConstants.MAX_LENGTH_50)]
        [Column(TypeName = "varchar")]
        public string Key { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsExpired { get; set; }
        public string Token { get; set; }
    }
}
