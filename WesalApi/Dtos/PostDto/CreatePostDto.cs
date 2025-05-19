namespace Wesal.Dtos.PostDto;

public class CreatePostDto
{
    public string postText { get; set; }
    public IFormFile? Image { get; set; }
}
