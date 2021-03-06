﻿using System;
using Microsoft.Azure.Documents.Client;

namespace DFC.Composite.Regions.Cosmos.Helper
{
    public static class DocumentDBHelper
    {
        private static Uri _documentCollectionUri;
        private static readonly string DatabaseId = Environment.GetEnvironmentVariable(Models.EnvironmentVariableNames.CosmosDatabaseId);
        private static readonly string CollectionId = Environment.GetEnvironmentVariable(Models.EnvironmentVariableNames.CosmosCollectionId);

        public static Uri CreateDocumentCollectionUri()
        {
            if (_documentCollectionUri != null)
            {
                return _documentCollectionUri;
            }

            _documentCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);

            return _documentCollectionUri;
        }

        public static Uri CreateDocumentUri(Guid RegionId)
        {
            return UriFactory.CreateDocumentUri(DatabaseId, CollectionId, RegionId.ToString());
        }

    }
}
