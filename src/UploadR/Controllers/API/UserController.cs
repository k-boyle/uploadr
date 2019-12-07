﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UploadR.Configurations;
using UploadR.Database;
using UploadR.Database.Enums;
using UploadR.Database.Models;
using UploadR.Interfaces;
using UploadR.Models;
using UploadR.Services;

namespace UploadR.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class UserController : Controller
    {
        private readonly UploadRContext _dbContext;
        private readonly EmailService _emails;
        private readonly RoutesConfiguration _routesConfiguration;

        public UserController(UploadRContext dbContext, IRoutesConfigurationProvider routesConfiguration, EmailService emails)
        {
            _dbContext = dbContext;
            _emails = emails;
            _routesConfiguration = routesConfiguration.GetConfiguration();
        }

        [HttpDelete, Route("token"), Authorize]
        public async Task<IActionResult> ResetAsync([FromQuery] bool reset = false)
        {
            var guid = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

            var token = await _dbContext.Tokens.Include(x => x.User).FirstOrDefaultAsync(x => x.UserGuid == guid);

            if (reset)
            {
                _dbContext.Remove(token);

                token = new Token
                {
                    CreatedAt = DateTime.Now,
                    Guid = Guid.NewGuid(),
                    UserGuid = guid,
                    TokenType = TokenType.User,
                    Revoked = false
                };

                await _dbContext.AddAsync(token);
            }
            else
            {
                token.Revoked = true;
                _dbContext.Update(token);
            }

            await _dbContext.SaveChangesAsync();

            await _emails.SendCustomActionAsync(token.User, "TOKEN_REVOKED");

            return reset
                ? Json(new { Token = token.Guid.ToString() }) as IActionResult
                : Ok("Token revoked");
        }

        [HttpPatch, Route("{guid}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleBlockUser(string guid, [FromQuery] bool block)
        {
            if (!Guid.TryParse(guid, out var userGuid))
            {
                return BadRequest("Invalid token supplied.");
            }

            var user = _dbContext.Users.Include(x => x.Token).FirstOrDefault(x => x.Guid == userGuid);
            if (user is null)
            {
                return BadRequest("Unknown token supplied.");
            }

            if (user.Token.TokenType == TokenType.Admin)
            {
                return BadRequest("Cannot modify admin token.");
            }

            user.Disabled = block;
            _dbContext.Update(user);

            await _dbContext.SaveChangesAsync();

            await _emails.SendCustomActionAsync(user, "ACCOUNT_BLOCKED");

            return Ok(block
                ? "User blocked."
                : "User unblocked.");
        }

        [HttpDelete, Route(""), Authorize]
        public async Task<IActionResult> DeleteUser()
        {
            var guid = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            var user = _dbContext.Users.FirstOrDefault(x => x.Guid == guid);

            _dbContext.Remove(user);

            await _dbContext.SaveChangesAsync();

            await _emails.SendCustomActionAsync(user, "ACCOUNT_REMOVED");

            return Ok("This account has been removed.");
        }

        [HttpPost, Route(""), AllowAnonymous]
        public async Task<IActionResult> CreateUser(UserCreateModel user)
        {
            if (!_routesConfiguration.UserRegisterRoute)
            {
                return Forbid();
            }

            if (user is null)
            {
                return BadRequest("Invalid form.");
            }

            var users = _dbContext.Users;

            if (!string.IsNullOrWhiteSpace(user.Email) && users.Any(x => x.Email == user.Email))
            {
                return BadRequest("A user with that email already exist.");
            }

            var dbUser = new User
            {
                Guid = Guid.NewGuid(),
                Email = user?.Email ?? "",
                CreatedAt = DateTime.Now,
                Disabled = false
            };

            var dbToken = new Token
            {
                CreatedAt = DateTime.Now,
                Guid = Guid.NewGuid(),
                UserGuid = dbUser.Guid,
                TokenType = TokenType.User,
                Revoked = false
            };

            await _dbContext.AddAsync(dbUser);
            await _dbContext.AddAsync(dbToken);

            await _dbContext.SaveChangesAsync();

            return Ok(new { Token = dbToken.Guid });
        }
    }
}