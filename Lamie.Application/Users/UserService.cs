using Lamie.Application.Common.Exceptions;
using Lamie.Application.Common.Interfaces;
using Lamie.Application.Users.Dtos;
using Lamie.Domain.Entities;

namespace Lamie.Application.Users
{
    public class UserService
    {
        private readonly ISysUserRepository _repo;

        public UserService(ISysUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<UserResponseDto>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync();

            return users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                FullName = u.FullName,
                Phone = u.Phone,
                IsActive = u.IsActive
            }).ToList();
        }

        public async Task<UserResponseDto> GetByIdAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id)
                ?? throw new NotFoundException("User", id);

            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Phone = user.Phone,
                IsActive = user.IsActive
            };
        }

        public async Task<int> CreateAsync(CreateUserDto dto)
        {
            if (await _repo.ExistsByUsernameAsync(dto.Username))
                throw new ConflictException("Username already exists");

            var user = new SysUser
            {
                Username = dto.Username,
                Password = dto.Password, // TODO: hash
                Email = dto.Email,
                FullName = dto.FullName,
                Phone = dto.Phone,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(user);
            return user.Id;
        }

        public async Task UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _repo.GetByIdAsync(id)
                ?? throw new NotFoundException("User", id);

            user.Email = dto.Email;
            user.FullName = dto.FullName;
            user.Phone = dto.Phone;
            user.IsActive = dto.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id)
                ?? throw new NotFoundException("User", id);

            await _repo.DeleteAsync(user);
        }
    }
}
