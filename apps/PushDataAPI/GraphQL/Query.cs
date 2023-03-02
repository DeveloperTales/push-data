using PushData.PushDataAPI.Models;
using PushData.PushDataAPI.Services;

namespace PushData.PushDataAPI.GraphQL;

public class Query
{
  private readonly DBService _dbService;
  public Query(DBService dbService)
  {
    _dbService = dbService;
  }
  public Book GetBook(string id) => _dbService.GetBook(Guid.Parse(id));

  public List<Book> GetBookSearch(string title) => _dbService.GetBookSearch(title);

  public List<Book> GetBooks() => _dbService.GetBooks();
}
