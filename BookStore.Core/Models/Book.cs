namespace BookStore.Core.Models;

public class Book
{
public Guid Id { get; set; } = Guid.NewGuid();
public string Title { get; set; } = string.Empty;
public Guid AuthorId { get; set; }
public DateTime Published { get; set; }
public string Genre { get; set; } = string.Empty;
public decimal Price { get; set; }
public int SoldCopies { get; set; }
}