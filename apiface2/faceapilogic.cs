
using DtoModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace faceapifunctions
{
 public  class faceapilogic
    {
        public faceapilogic()
        { 
        
        
        }

        public async System.Threading.Tasks.Task<User> getUser(Stream stream)
        {
            string requestBody = await new StreamReader(stream).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<User>(requestBody);
            return data;

        }
    }
}
