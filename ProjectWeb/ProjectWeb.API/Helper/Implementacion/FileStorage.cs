namespace ProjectWeb.API.Helper.Implementacion
{
    public class FileStorage : IFileStorage
    {
        private readonly IWebHostEnvironment _env;

        public FileStorage(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveImageAsync(string base64Image)
        {
            try
            {
                // 1️⃣ Limpiar encabezado si viene con data:image/...
                if (base64Image.Contains(","))
                    base64Image = base64Image[(base64Image.IndexOf(",") + 1)..];

                // 2️⃣ Decodificar base64
                byte[] imageBytes = Convert.FromBase64String(base64Image);

                // 3️⃣ Obtener ruta segura de wwwroot
                string webRootPath = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");

                // Crear carpeta wwwroot si no existe
                if (!Directory.Exists(webRootPath))
                    Directory.CreateDirectory(webRootPath);

                // Crear carpeta UserImagen dentro de wwwroot
                string uploadsPath = Path.Combine(webRootPath, "UserImagen");
                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                // 4️⃣ Nombre único
                string fileName = $"{Guid.NewGuid()}.jpg";
                string filePath = Path.Combine(uploadsPath, fileName);

                // 5️⃣ Guardar archivo
                await File.WriteAllBytesAsync(filePath, imageBytes);

                // 6️⃣ Devolver URL pública relativa correcta
                return $"/UserImagen/{fileName}";
            }
            catch (FormatException)
            {
                throw new ArgumentException("El formato Base64 no es válido.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al guardar la imagen: {ex.Message}");
            }
        }

        public async Task<string> UpdateImageAsync(string oldImageUrl, string newBase64Image)
        {
            // Eliminar la imagen antigua
            await DeleteImageAsync(oldImageUrl);
            // Guardar la nueva imagen
            return await SaveImageAsync(newBase64Image);
        }

        public Task<bool> DeleteImageAsync(string imageUrl)
        {
            try
            {
                string fileName = Path.GetFileName(imageUrl);
                string filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}
