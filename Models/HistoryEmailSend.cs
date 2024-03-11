using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BurgerApi.Models
{
    public partial class HistoryEmailSend
    {
        [Key]
        public int HistoryEmailId { get; set; }

        [Required]
        public required int UserId { get; set; }
        public required string UuidToken { get; set; }

        public DateTime ExpireAt { get; set; }

        [DefaultValue(false)]
        public bool Expired { get; set; }
        public DateTime? ExpiredAt { get; set; }

        public virtual User? User { get; set; }
    }
}
