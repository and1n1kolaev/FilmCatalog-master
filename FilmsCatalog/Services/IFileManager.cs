using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Services
{
    public interface IFileManager
    {
        byte[] Get(string fileName); 
        string Save(IFormFile file);
        bool Delete(string fileName);
    }
}
