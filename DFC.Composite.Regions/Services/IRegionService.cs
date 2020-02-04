using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.Composite.Regions.Models;

namespace DFC.Composite.Regions.Services
{
    public interface IRegionService
    {
        Task<List<Models.Region>> GetListAsync(string path);

        Task<Region> GetByIdAsync(Guid documentId);

        Task<Region> GetAsync(string path, Constants.PageRegions pageRegion);

        Task<Region> CreateAsync(Region region);

        Task<Region> ReplaceAsync(Region region);

        Task<bool> DeleteAsync(Guid documentId);
    }
}