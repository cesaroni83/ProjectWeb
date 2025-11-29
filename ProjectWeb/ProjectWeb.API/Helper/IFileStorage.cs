namespace ProjectWeb.API.Helper
{
    public interface IFileStorage
    {
        //Task<string> SaveImageAsync(string base64Image);
        //Task<string> UpdateImageAsync(string oldImageUrl, string newBase64Image);
        //Task<bool> DeleteImageAsync(string imageUrl);
        //Task<string> SaveImageFromUrlAsync(string imageUrl);
        /// <summary>
        /// Guarda una imagen en Base64 en la carpeta especificada dentro de wwwroot.
        /// </summary>
        /// <param name="base64Image">Imagen en Base64</param>
        /// <param name="folderName">Carpeta donde guardar la imagen (por defecto UserImagen)</param>
        /// <returns>URL pública de la imagen</returns>
        Task<string> SaveImageAsync(string base64Image, string folderName = "UserImagen");

        /// <summary>
        /// Actualiza una imagen existente borrando la anterior y guardando la nueva.
        /// </summary>
        /// <param name="oldImageUrl">URL de la imagen antigua</param>
        /// <param name="newBase64Image">Imagen nueva en Base64</param>
        /// <param name="folderName">Carpeta donde guardar la imagen (por defecto UserImagen)</param>
        /// <returns>URL pública de la nueva imagen</returns>
        Task<string> UpdateImageAsync(string oldImageUrl, string newBase64Image, string folderName = "UserImagen");

        /// <summary>
        /// Elimina una imagen de la carpeta especificada.
        /// </summary>
        /// <param name="imageUrl">URL de la imagen a eliminar</param>
        /// <param name="folderName">Carpeta donde se encuentra la imagen (por defecto UserImagen)</param>
        /// <returns>True si se eliminó, false si no existía o hubo error</returns>
        Task<bool> DeleteImageAsync(string imageUrl, string folderName = "UserImagen");

        /// <summary>
        /// Descarga una imagen desde una URL, la guarda en la carpeta especificada y devuelve su URL pública.
        /// </summary>
        /// <param name="imageUrl">URL de la imagen a descargar</param>
        /// <param name="folderName">Carpeta donde guardar la imagen (por defecto UserImagen)</param>
        /// <returns>URL pública de la imagen guardada</returns>
        Task<string> SaveImageFromUrlAsync(string imageUrl, string folderName = "UserImagen");
    }
}

