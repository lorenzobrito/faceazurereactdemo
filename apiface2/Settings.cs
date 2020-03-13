using Azure.Storage.Blobs;
using DtoModels;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CloudStorageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount;
using StorageException = Microsoft.Azure.Cosmos.Table.StorageException;

namespace apiface2
{
  public  class Settings
    {
        public static string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=nominashtlml;AccountKey=Yv37qkHCllh5MhwOu20RtURNkPqUg5SUf+M6Bnnm9hX6CluD+r+aOlVrZllL7y6giUKbBWUxfSYUj0AvGl46tQ==;EndpointSuffix=core.windows.net";

         static CloudStorageAccount CreateStorageAccountFromConnectionString()
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }

        public static async Task<CloudTable> CreateTableAsync(string tableName="User")
        {

            // Retrieve storage account information from connection string.
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString();

            // Create a table client for interacting with the table service
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

            Console.WriteLine("Create a Table for the demo");

            // Create a table client for interacting with the table service 
            CloudTable table = tableClient.GetTableReference(tableName);
            if (await table.CreateIfNotExistsAsync())
            {
                Console.WriteLine("Created Table named: {0}", tableName);
            }
            else
            {
                Console.WriteLine("Table {0} already exists", tableName);
            }

            Console.WriteLine();
            return table;
        }

        public static async Task<User> InsertOrMergeEntityAsync(CloudTable table, User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                // Create the InsertOrReplace table operation
                //entity.RowKey = "tras";
                TableOperation insertOrMergeOperation = TableOperation.Insert(entity);

                // Execute the operation.
                TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
                var insertedCustomer = result.Result as User;

                // Get the request units consumed by the current operation. RequestCharge of a TableResult is only applied to Azure Cosmos DB
                if (result.RequestCharge.HasValue)
                {
                    Console.WriteLine("Request Charge of InsertOrMerge Operation: " + result.RequestCharge);
                }

                return insertedCustomer;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
              
                throw;
            }
        }

       

    }
}
