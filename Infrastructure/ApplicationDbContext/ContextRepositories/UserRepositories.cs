using ApplicationDbContext.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ModelsEntity;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.ContextRepositories
{
    public class UserRepositories : IUserRepositories
    {
        private readonly AppDbContext context;

        public UserRepositories(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<User> GetUserByUserId(int userId)
        {
            try
            {
                if (userId > 0)
                {
                    return await context.Users.FindAsync(userId);
                }
                else
                {
                    return new User();
                }
            } catch(Exception e)
            {
                Console.WriteLine("Не найдено записи User, либо передано некорректное значение", e);
                return new User();
            }
        }
    }
}
