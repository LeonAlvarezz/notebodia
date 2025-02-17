using System.Net;
using System.Text;
using System.Text.Json;
using FluentDate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using notebodia_api.Dto;
using notebodia_api.Response;
using notebodia_api.Models;
using notebodia_api.Repositories;
using notebodia_api.Types;
using notebodia_api.Util;

namespace notebodia_api.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly SessionRepository _sessionRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserService(UserRepository userRepository, SessionRepository sessionRepository, ILogger<UserService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<User> GetUserById(Guid id)
        {
            var user = await _userRepository.GetUserById(id);

            if (user == null) throw new NotFoundException();

            return user;
        }

        public async Task<UserDto> UserSignup(CreateUserPayload user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Email)) throw new ArgumentException("Email is required.");
                return await _userRepository.UserSignup(user);
            }
            catch (DuplicateResourceException)
            {
                throw new DuplicateResourceException("User with the same email or username is already exists");
            }

        }

        public async Task<UserDto> UserLogin(CreateUserPayload payload)
        {
            var user = await _userRepository.CheckEmailAndUsername(payload.Username, payload.Email);
            if (user == null)
            {
                throw new UnauthorizedException("Invalid Credentials");
            }
            var isValid = PasswordHasher.VerifyPassword(payload.Password, user.Password);
            if (!isValid)
            {

                throw new UnauthorizedException("Invalid Credentials");
            }
            try
            {
                var token = EncryptionUtils.GenerateSessionToken();
                var tokenBytes = Encoding.UTF8.GetBytes(token);
                var sessionId = EncryptionUtils.EncodeHexLowerCase(tokenBytes);
                PrintUtils.PrettyPrint(token);
                var session = await _sessionRepository.CreateSessionAsync(new Session
                {
                    Id = sessionId,
                    UserId = user.Id,
                    ExpiresAt = DateTime.UtcNow.Add(15.Days())
                });

                CookieUtil.SetCookie(_httpContextAccessor.HttpContext!, "session_token", token, 15);

                return new UserDto
                {
                    Email = user.Email,
                    Id = user.Id,
                    Username = user.Username,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected Error");
                throw new InternalServerException();
            }

        }
        public async Task<UserDto> GetMe()
        {
            var token = CookieUtil.GetCookie(_httpContextAccessor.HttpContext!, "session_token");
            if (token == null)
            {
                throw new UnauthorizedException();
            }
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var sessionId = EncryptionUtils.EncodeHexLowerCase(tokenBytes);
            var session = await _sessionRepository.GetSessionByIdAsync(sessionId);
            if (session == null)
            {
                throw new UnauthorizedException();
            }
            return session.User;

        }
        public async Task<object> UserSignout()
        {
            var token = CookieUtil.GetCookie(_httpContextAccessor.HttpContext!, "session_token");
            if (token == null)
            {
                throw new UnauthorizedException();
            }
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var sessionId = EncryptionUtils.EncodeHexLowerCase(tokenBytes);
            var deleted = await _sessionRepository.DeleteSessionAsync(sessionId);
            if (!deleted)
            {
                throw new InternalServerException();
            }
            CookieUtil.DeleteCookie(_httpContextAccessor.HttpContext!, "session_token");
            return SimpleSuccess.Default;
        }
    }
}