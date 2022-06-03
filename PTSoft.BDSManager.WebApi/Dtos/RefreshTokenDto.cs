using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PTSoft.BDSManager.WebApi.Dtos;

public class RefreshTokenDto
{
    [Required]
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
}