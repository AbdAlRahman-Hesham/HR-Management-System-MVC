namespace MvcProject1.PL.FileManagers
{
    public static class FileManager
    {
        public static string UploadFile(IFormFile file,
            int id, FileType fileType = FileType.Images)
        {
            // Get the current working directory
            string currentPath = Directory.GetCurrentDirectory();

            // Generate a unique file name using GUID and the provided ID
            string name = $"Id_{id}_{Guid.NewGuid()}_{file.FileName}";

            // Construct the full file path
            string filePath = Path.Combine(currentPath, "wwwroot", 
                "Files", fileType.ToString(),
                $"{name}");

            // Ensure the directory exists
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Save the file to the specified path
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return name;
        }

        public static void DeleteFile(string name, 
            FileType fileType = FileType.Images)
        {
            // Get the current working directory
            string currentPath = Directory.GetCurrentDirectory();

            // Construct the full file path
            string filePath = Path.
                Combine(currentPath, 
                "wwwroot", "Files", fileType.ToString(), name);

            // Check if the file exists and delete it
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    public enum FileType
    {
        Images,
        Videos,
        Documents
    }
}
