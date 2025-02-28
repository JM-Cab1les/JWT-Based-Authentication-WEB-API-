using Azure.Identity;
using JWT_AUTH.Contexts;
using JWT_AUTH.Helper;
using JWT_AUTH.Models;
using Microsoft.EntityFrameworkCore;

namespace JWT_AUTH.Service
{
    public class AuthService
    {
        private readonly TEST_JMContext contexts_;
        private readonly IConfiguration configuration_;
        private readonly JWTHelper jwtHelper_;


        public AuthService(TEST_JMContext contexts, IConfiguration configuration, JWTHelper jWTHelper)
        {
            contexts_ = contexts;
            configuration_ = configuration;
            jwtHelper_ = jWTHelper;
        }

        //Register service
        public async Task<Register> Register(Register register_user)
        {
            var existingUser = await contexts_.Registers.FirstOrDefaultAsync(u => u.UserName == register_user.UserName);
            if (existingUser != null)
                throw new Exception("User already exists");

            string passwordHash = jwtHelper_.HashPassword(register_user.Password);

            var user = new Register { Name = register_user.Name, UserName = register_user.UserName, Password = passwordHash };


            contexts_.Registers.Add(user);
            await contexts_.SaveChangesAsync();
            return user;

        }

        // Login service
        public async Task<string> Login(Register register)
        {
            var user = await contexts_.Registers.FirstOrDefaultAsync(u => u.UserName == register.UserName);
            var hashPassword = jwtHelper_.VerifyPassword(register.Password, user.Password);
            if (user == null || !hashPassword)
                return null;

            return jwtHelper_.GenerateToken(user);
        }



    }
}
