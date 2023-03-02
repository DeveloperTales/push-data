using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PushData.PushDataAPI.Models;
using PushData.PushDataAPI.Services;
using PushData.PushDataAPI.SignalR;

namespace PushData.PushDataAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BooksController : ControllerBase
  {
    private readonly DBService _dbService;
    private readonly ITopicEventSender _eventSender;
    private readonly IHubContext<BookHub> _hubContext;

    public BooksController(DBService dbService, ITopicEventSender eventSender, IHubContext<BookHub> hubContext)
    {
      _dbService = dbService;
      _eventSender = eventSender;
      _hubContext = hubContext;
    }

    [HttpPost(Name = "PublishBook")]
    public async Task<Book> Post(Book book, CancellationToken cancellationToken)
    {
      var newBook = _dbService.AddBook(book);
      await _eventSender.SendAsync("PublishBook", newBook, cancellationToken);
      await _hubContext.Clients.All.SendAsync("PublishBook", newBook, cancellationToken);
      return newBook;
    }

    [HttpDelete(Name = "DeleteBook")]
    public async Task Delete(string bookId, CancellationToken cancellationToken)
    {
      _dbService.DeleteBook(Guid.Parse(bookId));
      await _eventSender.SendAsync("RemoveBook", bookId, cancellationToken);
      await _hubContext.Clients.All.SendAsync("RemoveBook", bookId, cancellationToken);
    }
  }
}
