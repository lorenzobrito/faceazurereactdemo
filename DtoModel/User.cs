using System;

namespace DtoModels
{
  
    public class User
    {

        public string dataUri { get; set; }
        public string name { get; set; }

        public string urlblob { get; set; }
        public string containername { get { return name; } }
        public Guid Guid;


    }
}
