namespace LibraryApp.Application.DTOs;

public class AuthorDto
{
    public Guid Id { get; set; }    
    public string FirstName {  get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string? Biography { get; set; }
    public DateTime? BirthDate { get; set; }
    public int? Age { get; set; }
    public int BookCount { get; set; }
}
