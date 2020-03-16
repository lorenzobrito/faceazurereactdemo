using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace facedemoconsoletest
{
   public class UploadPictures
    {

        public IEnumerable<string> fromDirectory(string url)
        {
           var files= System.IO.Directory.EnumerateFiles(url);
            return files;
        }
        public string ConvertImageto64(string Path)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(Path);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                return base64ImageRepresentation;
        }
        public void Proces(string directory)
        { 
        
        }
    }
}
