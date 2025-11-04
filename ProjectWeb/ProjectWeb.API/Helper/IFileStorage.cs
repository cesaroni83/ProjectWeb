namespace ProjectWeb.API.Helper
{
    public interface IFileStorage
    {
        Task<string> SaveImageAsync(string base64Image);
        Task<string> UpdateImageAsync(string oldImageUrl, string newBase64Image);
        Task<bool> DeleteImageAsync(string imageUrl);
        Task<string> SaveImageFromUrlAsync(string imageUrl);
    }
}
