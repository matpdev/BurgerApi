using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BurgerApi.Controllers.Auth.Dto;
using BurgerApi.Data;
using BurgerApi.Models;
using BurgerApi.Utils.Chipher;
using Microsoft.AspNetCore.Mvc;

namespace BurgerApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private BurgerContext db = new BurgerContext();

        [HttpPost("login")]
        public IActionResult AuthLogin([FromBody] AuthLoginDto loginDto)
        {
            var user = db.Users.FirstOrDefault(e => e.Email == loginDto.Email);

            if (user == null)
                return NotFound();

            if (loginDto.Password != null)
            {
                if (EncryptionHelper.ValidateCipher(loginDto.Password!, user.PasswordHash!))
                {
                    return Ok(
                        new AuthResponseDto(
                            user: user,
                            tokenItem: new JwtComponent().EncryptData(
                                user.UserId.ToString(),
                                user.Email!
                            )
                        )
                    );
                }
            }
            else
            {
                if (loginDto.IntegrationToken == user.IntegrationToken)
                {
                    return Ok(
                        new AuthResponseDto(
                            user: user,
                            tokenItem: new JwtComponent().EncryptData(
                                user.UserId.ToString(),
                                user.Email!
                            )
                        )
                    );
                }
            }

            return NotFound();
        }

        [HttpPost("register")]
        public async Task<IActionResult> AuthRegister([FromBody] AuthRegisterDto registerDto)
        {
            var userExist = db.Users.FirstOrDefault(e => e.Email == registerDto.Email);

            if (userExist != null)
                return Conflict();

            User user = new User
            {
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                Name = registerDto.Name,
                PasswordHash =
                    registerDto.Password != null
                        ? EncryptionHelper.Encrypt(registerDto.Password)
                        : null,
                Address = registerDto.Address,
                IntegrationToken = registerDto.IntegrationToken,
            };

            db.Users.Add(user);

            await db.SaveChangesAsync();

            return Ok(
                new AuthResponseDto(
                    user: user,
                    tokenItem: new JwtComponent().EncryptData(user.UserId.ToString(), user.Email!)
                )
            );
        }
    }
}
