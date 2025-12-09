namespace HandyHub.Helper;

public class Upload
{
    public static string UploadProfileImage ( string folderName, IFormFile file )
    {
        if (file == null || file.Length == 0)
            return null;

        try
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string ext = Path.GetExtension(file.FileName);
            string fileName = $"{Guid.NewGuid()}{ext}";
            string finalPath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(finalPath, FileMode.Create);
            file.CopyTo(stream);

            return fileName;
        }
        catch
        {
            return null;
        }
    }

    public static bool RemoveProfileImage ( string folderName, string fileName )
    {
        try
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
}
