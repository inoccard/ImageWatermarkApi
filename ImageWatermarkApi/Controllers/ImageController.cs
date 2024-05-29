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
    /// <summary>
    /// Aplica marca d'água na imagem
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("apply-watermark")]
    public IActionResult AddWatermark([FromBody] ImageRequest request)
    {
        if (IsNullOrEmpty(request) || IsStringContent(request))
            return BadRequest("dados inválidos");

        if (!IsBase64String(request.Base64Image))
            return BadRequest("A imagem fornecida não é uma string base64 válida");

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
    /// Converte imagem base64 para imagem real
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("convert-to-image")]
    public IActionResult ConvertBase64ToImage([FromBody] Base64ImageRequest request)
    {
        if (string.IsNullOrEmpty(request.Base64Image))
            return BadRequest("dados inválidos");

        if (!IsBase64String(request.Base64Image))
            return BadRequest("A imagem fornecida não é uma string base64 válida");

        try
        {
            var imageBytes = Convert.FromBase64String(request.Base64Image);
            using var ms = new MemoryStream(imageBytes);
            return File(ms.ToArray(), "image/png");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    #region private methods 

    private static bool IsNullOrEmpty(ImageRequest request)
    {
        return string.IsNullOrEmpty(request.Base64Image) ||
               string.IsNullOrEmpty(request.WatermarkText) ||
               string.IsNullOrEmpty(request.Position);
    }

    private static bool IsStringContent(ImageRequest request)
    {
        return request.Base64Image.Equals("string") ||
               request.WatermarkText.Equals("string") ||
               request.Position.Equals("string");
    }

    private static bool IsBase64String(string base64Image)
    {
        if (base64Image.Length % 4 != 0 ||
            base64Image.Contains(' ') ||
            base64Image.Contains('\t') ||
            base64Image.Contains('\r') ||
            base64Image.Contains('\n'))
            return false;

        return true;

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

    #endregion private methods
}

