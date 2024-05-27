using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using ImageWatermarkApi.Models;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.Fonts;

namespace ImageWatermarkApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    [HttpPost]
    public IActionResult AddWatermark([FromBody] ImageRequest request)
    {
        try
        {
            using Image image = LoadImage(request);

            PointF location = GetWatermarkPosition(request.Position, image);

            ApplyWatermark(request, image, location);

            EncodeImageToBase64(image, out string resultBase64);

            return Ok(resultBase64);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Carregar imagem
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private static Image LoadImage(ImageRequest request)
    {
        // Decodificar a imagem Base64
        byte[] imageBytes = Convert.FromBase64String(request.Base64Image);
        var image = Image.Load(imageBytes);
        return image;
    }

    /// <summary>
    /// Configurar a marca d'água
    /// </summary>
    /// <param name="position"></param>
    /// <param name="image"></param>
    /// <returns></returns>
    private static PointF GetWatermarkPosition(string position, Image image)
    {
        // Definir a posição da marca d'água
        return position.ToLower() switch
        {
            "topleft" => new PointF(10, 10),
            "topright" => new PointF(image.Width - 150, 10),
            "bottomleft" => new PointF(10, image.Height - 50),
            "bottomright" => new PointF(image.Width - 150, image.Height - 50),
            _ => new PointF(10, 10),
        };
    }

    /// <summary>
    /// Aplicar a marca d'água
    /// </summary>
    /// <param name="request"></param>
    /// <param name="image"></param>
    /// <param name="location"></param>
    private static void ApplyWatermark(ImageRequest request, Image image, PointF location)
    {
        Font font = SystemFonts.CreateFont("Times New Roman", 24);
        image.Mutate(ctx => ctx.DrawText(request.WatermarkText, font, Color.Black, location));
    }

    /// <summary>
    /// Codificar a imagem de volta para Base64
    /// </summary>
    /// <param name="image"></param>
    /// <param name="resultBase64"></param>
    private static void EncodeImageToBase64(Image image, out string resultBase64)
    {
        var ms = new MemoryStream();
        image.SaveAsPng(ms);
        resultBase64 = Convert.ToBase64String(ms.ToArray());
    }

}

