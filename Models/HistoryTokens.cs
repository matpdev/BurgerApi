using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BurgerApi.Models
{
    public partial class HistoryTokens
    {
        [Key]
        public int HistoryTokensId { get; set; }

        [Required]
        public required int UserId { get; set; }
        public required string JwtToken { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ExpireAt { get; set; }
        public bool Expired { get; set; }
        public DateTime? ExpiredAt { get; set; }

        public virtual User? User { get; set; }
    }
}
