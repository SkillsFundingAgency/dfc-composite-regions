using DFC.Composite.Regions.Cosmos.Client;
using DFC.Composite.Regions.Cosmos.Helper;
using DFC.Composite.Regions.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Composite.Regions.Cosmos.Provider
{
    public class DocumentDBProvider : IDocumentDBProvider
    {
        public async Task<List<Models.Region>> GetRegionsForPathAsync(string path)
        {
            var client = DocumentDBClient.CreateDocumentClient();

            if (client == null)
            {
                return null;
            }

            var collectionUri = DocumentDBHelper.CreateDocumentCollectionUri();

            var regionsQuery = client.CreateDocumentQuery<Models.Region>(collectionUri, new FeedOptions { EnableCrossPartitionQuery = true })
                                     .Where(so => so.Path == path)
                                     .AsDocumentQuery();

            var regions = new List<Models.Region>();

            while (regionsQuery.HasMoreResults)
            {
                var response = await regionsQuery.ExecuteNextAsync<Models.Region>();

                regions.AddRange(response);
            }

            return regions.Any() ? regions : null;
        }

        public async Task<Models.Region> GetRegionForPathAsync(string path, Constants.PageRegions pageRegion)
        {
            var client = DocumentDBClient.CreateDocumentClient();

            if (client == null)
            {
                return null;
            }

            var collectionUri = DocumentDBHelper.CreateDocumentCollectionUri();

            var regionForCustomerQuery = client
                ?.CreateDocumentQuery<Models.Region>(collectionUri, new FeedOptions { MaxItemCount = 1, EnableCrossPartitionQuery = true })
                .Where(x => x.Path == path && x.PageRegion == pageRegion)
                .AsDocumentQuery();

            if (regionForCustomerQuery == null)
            {
                return null;
            }

            var Regions = await regionForCustomerQuery.ExecuteNextAsync<Models.Region>();

            return Regions?.FirstOrDefault();
        }

        public async Task<Models.Region> GetRegionByIdAsync(Guid documentId)
        {
            var client = DocumentDBClient.CreateDocumentClient();

            if (client == null)
            {
                return null;
            }

            var collectionUri = DocumentDBHelper.CreateDocumentCollectionUri();

            var regionForCustomerQuery = client
                ?.CreateDocumentQuery<Models.Region>(collectionUri, new FeedOptions { MaxItemCount = 1, EnableCrossPartitionQuery = true })
                .Where(x => x.DocumentId == documentId)
                .AsDocumentQuery();

            if (regionForCustomerQuery == null)
            {
                return null;
            }

            var Regions = await regionForCustomerQuery.ExecuteNextAsync<Models.Region>();

            return Regions?.FirstOrDefault();
        }

        public async Task<ResourceResponse<Document>> CreateRegionAsync(Region region)
        {
            var client = DocumentDBClient.CreateDocumentClient();

            if (client == null)
            {
                return null;
            }

            var collectionUri = DocumentDBHelper.CreateDocumentCollectionUri();

            var response = await client.CreateDocumentAsync(collectionUri, region);

            return response;
        }

        public async Task<ResourceResponse<Document>> UpdateRegionAsync(Region region)
        {
            var client = DocumentDBClient.CreateDocumentClient();

            if (client == null)
            {
                return null;
            }

            var documentUri = DocumentDBHelper.CreateDocumentUri(region.DocumentId.Value);

            var response = await client.ReplaceDocumentAsync(documentUri, region);

            return response;
        }

        public async Task<ResourceResponse<Document>> DeleteRegionAsync(Guid documentId)
        {
            var client = DocumentDBClient.CreateDocumentClient();

            if (client == null)
            {
                return null;
            }

            var documentUri = DocumentDBHelper.CreateDocumentUri(documentId);

            var response = await client.DeleteDocumentAsync(documentUri, new RequestOptions() { PartitionKey = new PartitionKey(Undefined.Value) });

            return response;
        }
    }
}