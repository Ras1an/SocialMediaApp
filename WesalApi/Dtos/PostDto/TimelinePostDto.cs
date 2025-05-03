namespace Wesal.Dtos.PostDto;

public class TimelinePostDto
{
    public int postId { get; set; }
    public string userId { get; set; }
    public string postText { get; set; }
    public string postPhoto { get; set; }
    public DateTime createdAt { get; set; }
    public int likeCount { get; set; }
}