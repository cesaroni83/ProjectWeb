using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.API.Data;
using ProjectWeb.Shared.Account;
using ProjectWeb.Shared.Enums;
using ProjectWeb.Shared.Modelo.DTO.Ciudad;
using ProjectWeb.Shared.Modelo.DTO.User;
using ProjectWeb.Shared.Modelo.Entidades;

namespace ProjectWeb.API.Helper.Implementacion
{
    public class UserHelper : IUserHelper
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public UserHelper(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<User> GetUserAsync(string email)
        {
            var user = await _context.Users
                .Include(u => u!.Ciudades)
                .ThenInclude(c => c!.Provincias!)
                .ThenInclude(s => s!.Paises!)
                .FirstOrDefaultAsync(x => x.Email == email);
            return user!;
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Ciudades!)
                .ThenInclude(c => c.Provincias!)
                .ThenInclude(s => s.Paises!)
                .FirstOrDefaultAsync(x => x.Id == userId.ToString());
            return user!;
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginDTO model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<IdentityResult> AddOrUpdateUserWithPersonaAsync(User user)
        {
            // Buscar Persona existente por email
            var existingPersona = await _context.Tbl_Persona
                .FirstOrDefaultAsync(p => p.Email == user.Email);

            if (existingPersona == null)
            {
              // Crear Persona asociada solo para usuarios nuevos
                var persona = new Persona
                {
                    Id_user = user.Id,
                    Nombre = user.FirstName,
                    Apellido = user.LastName,
                    Id_ciudad = user.Id_ciudad,
                    Direccion_persona = user.Address,
                    Email = user.Email!,
                    Estado_persona = "A",
                    Tipo_persona=user.UserType.ToString()
                };

                _context.Tbl_Persona.Add(persona);
                var results = await _context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            else
            {
                // Actualizar usuario existente
                existingPersona.Nombre = user.FirstName;
                existingPersona.Apellido = user.LastName;
                existingPersona.Direccion_persona = user.Address;
                existingPersona.Id_ciudad = user.Id_ciudad;
                existingPersona.Telefono = user.PhoneNumber;
                existingPersona.Tipo_persona = user.UserType.ToString();
                _context.Tbl_Persona.Update(existingPersona);
                await _context.SaveChangesAsync();
                return IdentityResult.Success;
            }
        }

        public async Task<ExternalLoginInfo?> GetExternalLoginInfoAsync()
        {
            return await _signInManager.GetExternalLoginInfoAsync();
        }

        public async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey)
        {
            return await _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent: false);
        }

        public async Task<IdentityResult> AddLoginAsync(User user, UserLoginInfo loginInfo)
        {
            return await _userManager.AddLoginAsync(user, loginInfo);
        }

        public async Task<User> CreateUserFromExternalLoginAsync(ExternalLoginInfo info, string email)
        {
            var user = new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                var loginResult = await _userManager.AddLoginAsync(user, info);
                if (!loginResult.Succeeded)
                {
                    throw new Exception("Error al asociar el login externo al usuario.");
                }
            }

            return user;
        }

        

        public async Task<UsersDTO> GetUser(string Email)
        {
            var user = await _context.Users
                .Include(u => u!.Ciudades)
                .ThenInclude(c => c!.Provincias!)
                .ThenInclude(s => s!.Paises!)
                .FirstOrDefaultAsync(x => x.Email == Email);

            if (user == null)
                return null;
            // Mapeo manual de entidad -> DTO
            var userDto = new UsersDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Email = user.Email,
                UserType = user.UserType,
                Ciudades = new CiudadDTO
                {
                    Id_ciudad = user.Ciudades.Id_ciudad,
                    Nombre_ciudad = user.Ciudades.Nombre_ciudad
                },
                Id_ciudad = user.Id_ciudad
            };

            return userDto;
        }
    }
}
