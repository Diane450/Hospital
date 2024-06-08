using Hospital.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hospital.Services
{
    public static class DBCall
    {
        private static readonly Context _dbContext = new();

        public static Worker? Authorize(string login, string password)
        {
            return _dbContext.Workers
                .Include(w => w.JobTitle)
                .ThenInclude(j => j.Department)
                .Include(w => w.User)
                .ThenInclude(u => u.Role)
                .FirstOrDefault(u => u.User.Login == login && u.User.Password == password);
        }

        public static List<DrugDTO> GetDrugs()
        {
            return [.. _dbContext.Drugs
                .Include(d => d.Manufacturer)
                .Include(d => d.DrugProvider)
                .Include(d => d.Type)
                .Select(d => new DrugDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Manufacturer = d.Manufacturer,
                    DrugProvider = d.DrugProvider,
                    Count = d.Count,
                    Type = d.Type,
                    Photo = d.Photo
                })];
        }

        public static List<Manufacturer> GetManufacturers()
        {
            return [.. _dbContext.Manufacturers];
        }

        public static List<DrugProvider> GetDrugProviders()
        {
            return [.. _dbContext.DrugProviders];
        }

        public static List<DrugType> GetTypes()
        {
            return [.. _dbContext.DrugTypes];
        }

        public static List<DispensingDrug> GetDispensingDrugData(DateOnly[] DateRange)
        {
            var dispensingDrugsQuantities = _dbContext.DispensingDrugs
            .Where(d => d.Date >= DateRange[0] && d.Date <= DateRange[1])
            .Include(d => d.Drug)
            .GroupBy(di => di.DrugId)
            .Select(g => new DispensingDrug()
            {
                Drug = g.First().Drug,
                DrugId = g.Key,
                Count = g.Sum(di => di.Count)
            })
            .ToList();
            return dispensingDrugsQuantities;
        }

        public static List<ReceivingDrug> GetReceivingDrugData(DateOnly[] DateRange)
        {
            var receivingDrugsQuantities = _dbContext.ReceivingDrugs
            .Where(d => d.Date >= DateRange[0] && d.Date <= DateRange[1])
            .Include(d => d.Drug)
            .GroupBy(di => di.DrugId)
            .Select(g => new ReceivingDrug()
            {
                Drug = g.First().Drug,
                DrugId = g.Key,
                Count = g.Sum(di => di.Count)
            })
            .ToList();
            return receivingDrugsQuantities;
        }

        public static void ReceiveDrug(int drugId, int workerId, int count)
        {
            var drug = _dbContext.Drugs.First(d => d.Id == drugId);
            drug.Count += count;
            _dbContext.SaveChanges();

            var receiveDrug = new ReceivingDrug
            {
                DrugId = drugId,
                WorkerId = workerId,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Count = count
            };
            _dbContext.ReceivingDrugs.Add(receiveDrug);
            _dbContext.SaveChanges();
        }

        public static void DispenseDrug(int drugId, int workerId, int count)
        {
            var drug = _dbContext.Drugs.First(d => d.Id == drugId);
            drug.Count -= count;
            _dbContext.SaveChanges();

            var dispensingDrug = new DispensingDrug
            {
                DrugId = drugId,
                WorkerId = workerId,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Count = count
            };
            _dbContext.DispensingDrugs.Add(dispensingDrug);
            _dbContext.SaveChanges();
        }

        public static void DeleteDrug(DrugDTO drugDTO)
        {
            var drug = _dbContext.Drugs.First(d => d.Id == drugDTO.Id);
            _dbContext.Drugs.Remove(drug);
            _dbContext.SaveChanges();
        }

        public static int AddDrug(DrugDTO drugDTO)
        {
            var drug = new Drug
            {
                Name = drugDTO.Name,
                Manufacturer = drugDTO.Manufacturer,
                Photo = drugDTO.Photo,
                DrugProvider = drugDTO.DrugProvider,
                Type = drugDTO.Type,
                Count = drugDTO.Count
            };
            _dbContext.Drugs.Add(drug);
            _dbContext.SaveChanges();
            return drug.Id;
        }

        public static void EditDrug(DrugDTO drugDTO)
        {
            var drug = _dbContext.Drugs.First(d => d.Id == drugDTO.Id);

            drug.Name = drugDTO.Name;
            drug.Manufacturer = drugDTO.Manufacturer;
            drug.Photo = drugDTO.Photo;
            drug.DrugProvider = drugDTO.DrugProvider;
            drug.Type = drugDTO.Type;

            _dbContext.SaveChanges();
        }
    }
}
