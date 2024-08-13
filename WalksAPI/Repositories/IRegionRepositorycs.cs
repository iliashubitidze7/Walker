using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region> GetByIdAsync(Guid Id);
        Task<Region> CreateAsync(Region region);
        Task<Region?> UpdateAsync(Region region, Guid id);
        Task<Region?> DeleteAsync(Guid id);

    }
}
