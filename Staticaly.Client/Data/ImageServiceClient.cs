using SixLabors.ImageSharp;
using Microsoft.AspNetCore.Components.Forms;

namespace Staticaly.Client.Data
{
    public class ImageServiceClient
    {
    // En tu servicio ImageServiceClient

    public async Task<byte[]?> HandleFileSelected(InputFileChangeEventArgs e)
    {
      var file = e.File;

      // Verificar si el archivo es nulo
      if (file is null)
      {
        return null;
      }

      // Verificar la extensión del archivo
      var extension = Path.GetExtension(file.Name)?.ToLowerInvariant();
      if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
      {
        return null;
      }

      // Leer el archivo en un buffer de bytes
      var buffer = new byte[file.Size];
      await file.OpenReadStream().ReadAsync(buffer);

      // Validar la resolución de la imagen (opcional)
      using (var imageStream = new MemoryStream(buffer))
      using (var image = Image.Load(imageStream))
      {
        if (image.Width > 480 || image.Height > 480)
        {
          return null;
        }
      }

      return buffer;
    }


  }
}