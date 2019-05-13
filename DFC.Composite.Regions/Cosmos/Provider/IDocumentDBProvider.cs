using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.Composite.Regions.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace DFC.Composite.Regions.Cosmos.Provider
{
    public interface IDocumentDBProvider
    {
        Task<List<Models.Region>> GetRegionsForPathAsync(string path);

        Task<Models.Region> GetRegionByIdAsync(Guid documentId);

        Task<Models.Region> GetRegionForPathAsync(string path, Constants.PageRegions pageRegion);

        Task<ResourceResponse<Document>> CreateRegionAsync(Region region);

        Task<ResourceResponse<Document>> UpdateRegionAsync(Region region);

        Task<ResourceResponse<Document>> DeleteRegionAsync(Guid documentId);
    }
}