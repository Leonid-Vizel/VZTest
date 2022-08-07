using System.Text.RegularExpressions;

namespace VZTest.Instruments
{
    public static class HostingFileSaver
    {
        private const int ImageMinimumBytes = 512;
        private static readonly string[] AllowedContentTypes = new string[] { "image/jpg", "image/jpeg", "image/pjpeg", "image/gif", "image/x-png", "image/png" };
        private static readonly string[] AllowedExtensions = new string[] { ".jpg", ".png", ".gif", ".jpeg" };

        public static async Task<string> Save(IFormFile file, IWebHostEnvironment environment)
        {
            if (!ConfirmFileIsImage(file))
            {
                return "";
            }
            string SaveDirectoryPath = Path.Combine(environment.ContentRootPath,"Images");
            if (!Directory.Exists(SaveDirectoryPath))
            {
                Directory.CreateDirectory(SaveDirectoryPath);
            }
            string Extention = Path.GetExtension(file.FileName);
            string FileName = $"{Guid.NewGuid()}{Extention}";
            string SavePath = Path.Combine(SaveDirectoryPath, FileName);
            while (File.Exists(SavePath))
            {
                FileName = $"{Guid.NewGuid()}{Extention}";
                SavePath = Path.Combine(SaveDirectoryPath, FileName);
            }
            try
            {
                using (var createStream = new FileStream(SavePath, FileMode.Create))
                {
                    await file.CopyToAsync(createStream);
                }
            }
            catch (Exception) { return ""; }
            return FileName;
        }

        public static bool ConfirmFileIsImage(IFormFile file)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (!AllowedContentTypes.Contains(file.ContentType.ToLower()))
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (!AllowedExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!file.OpenReadStream().CanRead)
                {
                    return false;
                }
                //------------------------------------------
                //check whether the image size exceeding the limit or not
                //------------------------------------------ 
                if (file.Length < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                file.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
