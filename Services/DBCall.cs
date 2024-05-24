using Hospital.Models;
using Hospital.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static List<DrugDTO> GetDrugs()
        {
            return _dbContext.Drugs
                .Include(d=>d.Manufacturer)
                .Include(d=>d.DrugProvider)
                .Include(d=>d.Type)
                .Select(d=>new DrugDTO
                { 
                    Id = d.Id,
                    Name = d.Name,
                    Manufacturer = d.Manufacturer,
                    DrugProvider = d.DrugProvider,
                    Count = d.Count,
                    Type = d.Type,
                    Photo = d.Photo
                }).ToList();
        }

        public static List<Manufacturer> GetManufacturers()
        {
            return _dbContext.Manufacturers.ToList();
        }

        public static List<DrugProvider> GetDrugProviders()
        {
            return _dbContext.DrugProviders.ToList();
        }

        public static List<DrugType> GetTypes()
        {
            return _dbContext.DrugTypes.ToList();
        }
    }
}
