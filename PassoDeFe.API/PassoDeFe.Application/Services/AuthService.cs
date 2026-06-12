using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassoDeFe.Application.DTOs;
using PassoDeFe.Application.Interfaces;
using PassoDeFe.Domain.Entities;
using PassoDeFe.Domain.Interfaces;
using BCrypt.Net;

namespace PassoDeFe.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {

            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser != null)
                throw new Exception("Email já cadastrado.");


            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _userRepository.AddAsync(user);

            return new AuthResponse
            {
                Name = user.Name,
                Email = user.Email,
                Token = string.Empty // vazio agora, o JWT vem depois apenas

            };

        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new Exception("Credenciais inválidas.");
            return new AuthResponse
            {
                Name = user.Name,
                Email = user.Email,
                Token = string.Empty // vazio agora, o JWT vem depois apenas
            };
        }
    }
}