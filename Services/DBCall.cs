using Hospital.Models;
using Hospital.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Services
{
    public static class DBCall
    {
        private static readonly Ispr2438IbragimovaDmHospitalContext _dbContext = new();

        public static User Authorize(string login, string password)
        {
            return _dbContext.Users
                .Include(u=>u.Role)
                .FirstOrDefault(u => u.Login == login && u.Password == password);
        }
    }
}
