using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Services
{
    public class DiskFileManager : IFileManager
    {
        private readonly string _folderPath;
        public DiskFileManager(string folderPath)
        {
            _folderPath = folderPath;
        }

        public string Save(IFormFile file)
        {
            try
            {
                if (file is null)
                    throw new ArgumentNullException(nameof(file));

                if (!Directory.Exists(_folderPath))
                    Directory.CreateDirectory(_folderPath);

                string fileName = Path.GetFileName(file.FileName);
                string uniqueFileName = $"{Guid.NewGuid()}{fileName}";

                string path = Path.Combine(_folderPath, uniqueFileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return uniqueFileName;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool Delete(string fileName)
        {
            try
            {
                if (fileName is null)
                    throw new ArgumentNullException(nameof(fileName));

                string path = Path.Combine(_folderPath, fileName);
                if (File.Exists(path))
                    File.Delete(path);   
                
            }
            catch (Exception ex)
            {

                throw;
            }

            return true;
        }

        public byte[] Get(string fileName)
        {
            if (fileName is null)
                throw new ArgumentNullException(nameof(fileName));

            string path = Path.Combine(_folderPath, fileName);
            if (File.Exists(path))
                return File.ReadAllBytes(path);

            return Array.Empty<byte>();
        }
    }
}
