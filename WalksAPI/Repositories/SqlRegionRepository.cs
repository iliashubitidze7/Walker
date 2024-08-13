using Microsoft.EntityFrameworkCore;
using WalksAPI.Data;
using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories
{
    public class SqlRegionRepository : IRegionRepository
    {
        private readonly ApplicationDbContext dbcontext;

        public SqlRegionRepository(ApplicationDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbcontext.Regions.AddAsync(region);
            await dbcontext.SaveChangesAsync();

            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion = await dbcontext.Regions.FirstOrDefaultAsync(u => u.Id == id);
            if (existingRegion == null) {
                return null;
            }

            dbcontext.Remove(existingRegion);
            dbcontext.SaveChangesAsync();

            return existingRegion;

        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbcontext.Regions.ToListAsync();

        }

        public async Task<Region> GetByIdAsync(Guid Id)
        {
            return await dbcontext.Regions.FirstOrDefaultAsync(u => u.Id == Id);
        }

        public async Task<Region> UpdateAsync(Region region, Guid id)
        {
            var existingRegion = await dbcontext.Regions.FirstOrDefaultAsync(u => u.Id == id);

            if (existingRegion != null) {
                return null;
            }

            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            await dbcontext.SaveChangesAsync();
            return existingRegion;
        }
    }
}
