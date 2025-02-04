namespace AdminPanel.Helpers
{
    public static class PictureSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
          
            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", folderName);
           
            var filePath = Path.Combine(FolderPath, file.FileName);

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                if (!File.Exists(filePath))
                file.CopyTo(fs);
            }

            return "images/products/"+ file.FileName;

        }
        public static void DeleteFile(string folderName, string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", folderName,fileName);

            if (!string.IsNullOrEmpty(fileName) && File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
