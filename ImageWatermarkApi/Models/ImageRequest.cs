namespace ImageWatermarkApi.Models;

public record ImageRequest(string Base64Image, string WatermarkText, string Position);
