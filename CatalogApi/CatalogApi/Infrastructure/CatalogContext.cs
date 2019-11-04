using CatalogApi.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogApi.Infrastructure
{
    public class CatalogContext
    {
        private IConfiguration configuration;
        private IMongoDatabase database;
        
        public CatalogContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            var connectionString = configuration.GetValue<String>("MongoSettings:ConnectionString");
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(connectionString);
            MongoClient client = new MongoClient(settings);
            if(client!= null)
            {
                // database with NAME catalogDb is created
                this.database = client.GetDatabase(configuration.GetValue<string>("MongoSettings:Database"));              
            }
        } 
        public IMongoCollection<CatalogItem> Catalog
        {
            get
            {
                return this.database.GetCollection<CatalogItem>("products"); // products collection(table) is created
            }
        }

    }
}
