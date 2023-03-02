using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PushData.PushDataAPI.Models;
using PushData.PushDataAPI.Services;
using PushData.PushDataAPI.SignalR;

namespace PushData.PushDataAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BooksRandomController : ControllerBase
  {
    private readonly DBService _dbService;
    private readonly ITopicEventSender _eventSender;
    private readonly IHubContext<BookHub> _hubContext;

    public BooksRandomController(DBService dbService, ITopicEventSender eventSender, IHubContext<BookHub> hubContext)
    {
      _dbService = dbService;
      _eventSender = eventSender;
      _hubContext = hubContext;
    }

    [HttpPost(Name = "PublishRandomBook")]
    public async Task<Book> Post(CancellationToken cancellationToken)
    {
      var newBook = _dbService.AddRandomBook();
      await _eventSender.SendAsync("PublishBook", newBook, cancellationToken);
      await _hubContext.Clients.All.SendAsync("PublishBook", newBook, cancellationToken);
      return newBook;
    }

    [HttpDelete(Name = "RemoveRandomBook")]
    public async Task Delete(CancellationToken cancellationToken)
    {
      var oldBook = _dbService.RemoveRandomBook();
      await _eventSender.SendAsync("RemoveBook", oldBook.Id.ToString(), cancellationToken);
      await _hubContext.Clients.All.SendAsync("RemoveBook", oldBook.Id.ToString(), cancellationToken);
    }
  }
}
