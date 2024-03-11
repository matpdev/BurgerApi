using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BurgerApi.Models;

namespace BurgerApi.Controllers.Auth.Dto
{
    public class AuthLoginDto
    {
        public string Email { get; set; } = "";
        public string? Password { get; set; }
        public string? IntegrationToken { get; set; }

        public override string ToString()
        {
            return $"Email={Email};Password={Password};IntegrationToken={IntegrationToken}";
        }
    }

    public class AuthResponseDto : User
    {
        public string Token { get; set; }
        public new bool IsConfirmed { get; set; }

        public AuthResponseDto(User user, string tokenItem)
        {
            UserId = user.UserId;
            Email = user.Email;
            Name = user.Name;
            UserId = user.UserId;
            Address = user.Address;
            PhoneNumber = user.PhoneNumber;
            IntegrationToken = user.IntegrationToken;
            Token = tokenItem;
            CreatedAt = user.CreatedAt;
            UpdatedAt = user.UpdatedAt;
            LastLogin = user.LastLogin;
            DeletedAt = user.DeletedAt;
            IsConfirmed = user.IsConfirmed;
        }
    }

    public class AuthRegisterDto
    {
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Password { get; set; }
        public string? IntegrationToken { get; set; }

        public override string ToString()
        {
            return $"Email={Email};Password={Password};IntegrationToken={IntegrationToken}";
        }
    }
}
