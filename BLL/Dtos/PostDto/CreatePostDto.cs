namespace Wesal.Dtos.PostDto;
using Microsoft.AspNetCore.Http;
public class CreatePostDto
{
    public string? postText { get; set; }
    public IFormFile? Image { get; set; }
}
