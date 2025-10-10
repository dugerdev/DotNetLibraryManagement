namespace LibraryApp.Application.DTOs;

public class BookDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PageCount {  get; set; } 
    public DateTime PublishedDate { get; set; }
    public int AvailableCopies {  get; set; }
    public int TotalCopies { get; set; }
    public bool IsAvailable { get; set; }
    public string AuthorName {  get; set; } = string.Empty;
    public string CategoryName {  get; set; } = string.Empty;
}
