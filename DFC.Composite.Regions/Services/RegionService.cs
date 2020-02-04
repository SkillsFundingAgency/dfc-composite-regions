using DFC.Composite.Regions.Cosmos.Provider;
using DFC.Composite.Regions.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Composite.Regions.Services
{
    public class RegionService : IRegionService
    {
        private readonly IDocumentDBProvider _documentDbProvider;

        public RegionService(IDocumentDBProvider documentDbProvider)
        {
            _documentDbProvider = documentDbProvider;
        }

        public async Task<List<Region>> GetListAsync(string path)
        {
            var results = await _documentDbProvider.GetRegionsForPathAsync(path);

            return results;
        }

        public async Task<Region> GetByIdAsync(Guid documentId)
        {
            var result = await _documentDbProvider.GetRegionByIdAsync(documentId);

            return result;
        }

        public async Task<Region> GetAsync(string path, Constants.PageRegions pageRegion)
        {
            var result = await _documentDbProvider.GetRegionForPathAsync(path, pageRegion);

            return result;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            region.DocumentId = null;
            region.IsHealthy = true;
            region.DateOfRegistration = DateTime.UtcNow;
            region.LastModifiedDate = DateTime.UtcNow;

            var response = await _documentDbProvider.CreateRegionAsync(region);
           
            return response.StatusCode == HttpStatusCode.Created ? (dynamic)response.Resource : null;
        }

        public async Task<Region> ReplaceAsync(Region region)
        {
            region.LastModifiedDate = DateTime.UtcNow;

            var response = await _documentDbProvider.UpdateRegionAsync(region);

            return response.StatusCode == HttpStatusCode.OK ? (dynamic)response.Resource : null;
        }

        public async Task<bool> DeleteAsync(Guid documentId)
        {
            var response = await _documentDbProvider.DeleteRegionAsync(documentId);

            return response.StatusCode == HttpStatusCode.NoContent ? true : false;
        }
    }
}
