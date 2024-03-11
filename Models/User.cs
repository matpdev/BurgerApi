using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BurgerApi.Models;

public partial class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Email { get; set; }

    [JsonIgnore]
    public string? PasswordHash { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? IntegrationToken { get; set; }

    public DateTime? LastLogin { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }

    [DefaultValue(false)]
    public bool IsConfirmed { get; set; }

    [JsonIgnore]
    public virtual ICollection<Establishment> Establishments { get; set; } =
        new List<Establishment>();

    [JsonIgnore]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
