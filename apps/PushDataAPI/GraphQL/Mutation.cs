using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.SignalR;
using PushData.PushDataAPI.Models;
using PushData.PushDataAPI.Services;
using PushData.PushDataAPI.SignalR;

namespace PushData.PushDataAPI.GraphQL;

public class Mutation
{
  private readonly DBService _dbService;
  private readonly ITopicEventSender _eventSender;
  private readonly IHubContext<BookHub> _hubContext;

  public Mutation(DBService dbService, ITopicEventSender eventSender, IHubContext<BookHub> hubContext)
  {
    _dbService = dbService;
    _eventSender = eventSender;
    _hubContext = hubContext;
  }

  public async Task<Book> PublishBook(Book book, CancellationToken cancellationToken)
  {
    var newBook = _dbService.AddBook(book);
    await _eventSender.SendAsync(nameof(PublishBook), newBook, cancellationToken);
    await _hubContext.Clients.All.SendAsync("PublishBook", newBook, cancellationToken);
    return newBook;
  }

  public async Task<string> RemoveBook(string bookId, CancellationToken cancellationToken)
  {
    _dbService.DeleteBook(Guid.Parse(bookId));
    await _eventSender.SendAsync(nameof(RemoveBook), bookId, cancellationToken);
    await _hubContext.Clients.All.SendAsync("RemoveBook", bookId, cancellationToken);

    return bookId;
  }
}
