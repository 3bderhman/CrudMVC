using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Crud.BL.Helper
{
    public static class Files
    {
        public static string UploadFile(IFormFile File, string FolderName)
        {
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Files", FolderName);
            string FileName = Guid.NewGuid() + Path.GetFileName(File.FileName);
            string FinalPath = Path.Combine(FolderPath, FileName);
            using (var stream = new FileStream(FinalPath, FileMode.Create))
            {
                File.CopyTo(stream);
            }
            return FileName;
        }
        public static string RemoveFile(string FolderName, string FileName)
        {
            var CurrentDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Files", FolderName, FileName);
            if (File.Exists(CurrentDirectory))
            {
                File.Delete(CurrentDirectory);
            }
            return "Done";
        }
    }
}
