using PushData.PushDataAPI.Models;

namespace PushData.PushDataAPI.Services;

public class DBService
{
  private readonly List<Book> _books;
 
  public DBService()
  {
    _books = new List<Book>
        {
            new Book(Guid.NewGuid(), "Guards! Guards!", new Author("Terry Pratchett")),
            new Book(Guid.NewGuid(), "Wyrd Sisters", new Author("Terry Pratchett")),
            new Book(Guid.NewGuid(), "Thrawn Ascendancy (Book 1: Chaos Rising)", new Author("Timothy Zahn")),
            new Book(Guid.NewGuid(), "Thrawn Ascendancy (Book II: Greater Good)", new Author("Timothy Zahn")),
            new Book(Guid.NewGuid(), "The Fellowship of the Ring", new Author("J. R. R. Tolkien")),
            new Book(Guid.NewGuid(), "The Two Towers", new Author("J. R. R. Tolkien")),
            new Book(Guid.NewGuid(), "The Return of the King", new Author("J. R. R. Tolkien")),
            new Book(Guid.NewGuid(), "The Blinding Knife", new Author("Brent Weeks")),
            new Book(Guid.NewGuid(), "The Broken Eye", new Author("Brent Weeks")),
        };
  }

  public List<Book> GetBooks()
  {
    return _books.OrderBy(book => book.Author?.Name).ToList();
  }

  public Book AddBook(Book book)
  {
    var newBook = book with
    {
      Id = Guid.NewGuid(),
    };

    _books.Add(newBook);

    return newBook;
  }

  public Book AddRandomBook()
  {
    var book = new Book(
      Guid.NewGuid(),
      Faker.Lorem.Words(1).FirstOrDefault(),
      new Author(Faker.Name.FullName())
      );

    _books.Add(book);

    return book;
  }

  public Book? RemoveRandomBook()
  {
    if (_books.Count == 0)
    {
      return null;
    }

    var randomIndex = new Random().Next(_books.Count);
    var oldBook = _books[randomIndex];
    _books.RemoveAt(randomIndex);

    return oldBook;
  }

  public void DeleteBook(Guid bookId)
  {
    var book = GetBook(bookId);
    if (book == null)
    {
      throw new ArgumentNullException(nameof(book));
    }

    _books.Remove(book);
  }

  public Book? GetBook(Guid id)
  {
    return _books.FirstOrDefault(b => b.Id == id);
  }

  public List<Book> GetBookSearch(string title)
  {
    return _books.Where(b => b.Title.ToLower().Contains(title.ToLower())).ToList();
  }
}
