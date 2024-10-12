using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BurgerApi.Configures;
using BurgerApi.Controllers.Auth.Dto;
using BurgerApi.Data;
using BurgerApi.Models;
using BurgerApi.Services;
using BurgerApi.Utils.Chipher;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BurgerApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly BurgerContext db = new();
        private readonly EmailSender emailSender = new EmailSender();

        [HttpPost("login")]
        public async Task<IActionResult> AuthLogin([FromBody] AuthLoginDto loginDto)
        {
            var user = db.Users.FirstOrDefault(e => e.Email == loginDto.Email);

            if (user == null)
                return NotFound();

            if (loginDto.Password != null)
            {
                if (EncryptionHelper.ValidateCipher(loginDto.Password!, user.PasswordHash!))
                {
                    var token = new JwtComponent().EncryptData(user.UserId.ToString(), user.Email!);
                    HistoryTokens historyTokens = new HistoryTokens
                    {
                        UserId = user.UserId,
                        JwtToken = token,
                        CreatedAt = DateTime.UtcNow,
                        ExpireAt = DateTime.UtcNow.AddHours(8),
                    };

                    db.HistoryTokens.Where(e => e.UserId == user.UserId)
                        .ExecuteUpdate(e => e.SetProperty(data => data.Expired, true));

                    db.HistoryTokens.Where(e => e.UserId == user.UserId)
                        .ExecuteUpdate(e => e.SetProperty(data => data.ExpiredAt, DateTime.UtcNow));

                    db.Users.Where(e => e.UserId == user.UserId)
                        .ExecuteUpdate(a => a.SetProperty(u => u.LastLogin, DateTime.UtcNow));

                    await db.SaveChangesAsync();

                    db.HistoryTokens.Add(historyTokens);

                    await db.SaveChangesAsync();
                    return Ok(new AuthResponseDto(user: user, tokenItem: token));
                }
            }
            else
            {
                if (loginDto.IntegrationToken == user.IntegrationToken)
                {
                    var token = new JwtComponent().EncryptData(user.UserId.ToString(), user.Email!);
                    HistoryTokens historyTokens = new HistoryTokens
                    {
                        UserId = user.UserId,
                        JwtToken = token,
                        CreatedAt = DateTime.UtcNow,
                        ExpireAt = DateTime.UtcNow.AddHours(8),
                    };

                    db.HistoryTokens.Add(historyTokens);
                    db.HistoryTokens.Where(e =>
                            e.UserId == user.UserId && e.ExpireAt >= DateTime.UtcNow
                        )
                        .ExecuteUpdate(e => e.SetProperty(data => data.Expired, true));
                    db.HistoryTokens.Where(e =>
                            e.UserId == user.UserId && e.ExpireAt >= DateTime.UtcNow
                        )
                        .ExecuteUpdate(e => e.SetProperty(data => data.ExpiredAt, DateTime.UtcNow));

                    return Ok(new AuthResponseDto(user: user, tokenItem: token));
                }
            }

            return NotFound();
        }

        [Authorization]
        [HttpPost("refreshtoken")]
        public async Task<IActionResult> AuthRefreshToken()
        {
            HttpContext context = HttpContext;
            int userId = int.Parse(context.Items["userId"].ToString());
            return Ok();
        }

        // [Authorization]
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
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                LastLogin = DateTime.UtcNow,
            };

            db.Users.Add(user);

            await db.SaveChangesAsync();

            var userData = db.Users.First(e => e.Email == registerDto.Email);
            var token = new JwtComponent().EncryptData(user.UserId.ToString(), user.Email!);
            var uuid = Guid.NewGuid().ToString();

            HistoryTokens historyTokens = new HistoryTokens
            {
                UserId = userData.UserId,
                JwtToken = token,
                CreatedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddHours(8),
            };

            db.HistoryTokens.Add(historyTokens);

            HistoryEmailSend emailSend =
                new()
                {
                    UserId = userData.UserId,
                    ExpireAt = DateTime.UtcNow.AddMinutes(30),
                    UuidToken = uuid,
                };

            db.HistoryEmailSends.Add(emailSend);

            await db.SaveChangesAsync();

            emailSender.SendEmail(user.Email, user.Name, uuid);

            return Ok(new AuthResponseDto(user: user, tokenItem: token));
        }

        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string key)
        {
            var histoyFinded = await db.HistoryEmailSends.FirstOrDefaultAsync(e =>
                e.UuidToken == key
                && DateTime.UtcNow <= e.ExpireAt
                && e.ExpiredAt == null
                && e.Expired == false
            );

            if (histoyFinded != null)
            {
                db.HistoryEmailSends.ExecuteUpdate(update =>
                    update.SetProperty(data => data.Expired, true)
                );
                db.HistoryEmailSends.ExecuteUpdate(update =>
                    update.SetProperty(data => data.ExpiredAt, DateTime.UtcNow)
                );
                db.Users.Where(e => e.UserId == histoyFinded.UserId)
                    .ExecuteUpdate(update => update.SetProperty(data => data.IsConfirmed, true));

                await db.SaveChangesAsync();

                return Ok("Email Confirmed");
            }

            return NotFound();
        }

        [Authorization]
        [HttpPost("updateuser")]
        public async Task<IActionResult> UpdateUser(AuthUpdateDto updateDto)
        {
            HttpContext context = HttpContext;
            int userId = int.Parse(context.Items["userId"].ToString());
            var lastUserData = db.Users.FirstOrDefault(e => e.UserId == userId);

            db.ChangeTracker.Clear();

            User user =
                new()
                {
                    UserId = lastUserData.UserId,
                    Email = updateDto.Email,
                    PhoneNumber = updateDto.PhoneNumber,
                    Name = updateDto.Name,
                    PasswordHash = lastUserData.PasswordHash,
                    Address = updateDto.Address,
                    IntegrationToken = lastUserData.IntegrationToken,
                    CreatedAt = lastUserData.CreatedAt,
                    UpdatedAt = DateTime.UtcNow,
                    LastLogin = lastUserData.LastLogin,
                };

            db.Users.Update(user);

            await db.SaveChangesAsync();

            return Ok(user);
        }

        [Authorization]
        [HttpGet]
        public async Task<IActionResult> GetMe()
        {
            HttpContext context = HttpContext;
            var user = db.Users.FirstOrDefault(e =>
                e.UserId == int.Parse(context.Items["userId"].ToString())
            );

            return Ok(user);
        }
    }
}
