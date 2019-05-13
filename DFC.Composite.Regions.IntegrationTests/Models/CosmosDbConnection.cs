using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.Composite.Regions.IntegrationTests.Models
{
    /// <summary>
    /// Used to supply Cosmos DB connection values from app settings
    /// </summary>
    public class CosmosDbConnection
    {
        /// <summary>
        /// Cosmos DB - Endpoint
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Cosmos DB - Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Cosmos DB - Database Id
        /// </summary>
        public string DatabaseId { get; set; }

        /// <summary>
        /// Cosmos DB - Collection Id
        /// </summary>
        public string CollectionId { get; set; }
    }
}
