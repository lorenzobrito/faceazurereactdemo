using Microsoft.Azure.Cosmos.Table;
using System;

namespace DtoModels
{
  
    public class User : TableEntity
    {
        public User()
        {

        }
        public User(User copy) : this(copy.name,copy.dataUri)
        {

            this.dataUri = copy.dataUri;
        }
        public User(string name, string urlblob)
        {
            PartitionKey = name;
            this.name = name;
            this.dataUri = urlblob;
            RowKey = Guid.NewGuid().ToString();
        }
        public string dataUri { get; set; }
        
        public string name { get; set; }

  


    }
}
