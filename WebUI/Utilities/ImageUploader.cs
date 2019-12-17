using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebUI.Utilities
{
    public class ImageUploader
    {
        public static bool UploadImage(IFormFile file, string path)
        {
            try
            {
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse
                        (file.ContentDisposition).FileName.Trim('"');

                    using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            } catch (Exception)
            {
                return false;
            }
        }
    }
}
