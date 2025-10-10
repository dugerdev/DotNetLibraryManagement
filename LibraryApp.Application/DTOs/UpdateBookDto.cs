namespace LibraryApp.Application.DTOs;

public class UpdateBookDto
{
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int PageCount { get; set; }
    public DateTime PublishedDate { get; set; }
    public int TotalCopies { get; set; }
    public Guid AuthorId { get; set; }
    public Guid CategoryId { get; set; }
}
