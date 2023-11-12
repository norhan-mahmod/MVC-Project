namespace Demo.PL.Helper
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file , string folderName)
        {
            // 1. Get Path (Location) of folderName
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", folderName);
            // 2. Get file name and make it unique 
            var fileName = $"{Guid.NewGuid()}-{Path.GetFileName(file.FileName)}";
            // 3. Get file path
            var filePath = Path.Combine(folderPath, fileName);
            // dealing with files is unManaged resources [CLR will not open or close]
            // so we use (using keyword)
            using var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);
            return fileName;
        }
        public static void DeleteFile(string fileName, string folderName)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", folderName);
            var filePath = Path.Combine(folderPath, fileName);
            File.Delete(filePath);
        }
    }
}
