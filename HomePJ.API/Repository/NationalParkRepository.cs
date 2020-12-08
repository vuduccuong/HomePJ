using HomePJ.API.Data;
using HomePJ.API.Models;
using HomePJ.API.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomePJ.API.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public NationalParkRepository(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _dbContext.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _dbContext.Remove(nationalPark);
            return Save();
        }

        public NationalPark GetNationalPark(int nationalParkID)
        {
            return _dbContext.NationalParks.FirstOrDefault(nationals => nationals.Id == nationalParkID);
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _dbContext.NationalParks.OrderBy(n=>n.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
            return _dbContext.NationalParks.Any(n => n.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool NationalParkExists(int id)
        {
            return _dbContext.NationalParks.Any(n => n.Id == id);
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _dbContext.Update(nationalPark);
            return Save();
        }
    }
}
