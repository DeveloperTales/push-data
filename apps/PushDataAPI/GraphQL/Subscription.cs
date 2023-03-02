using HotChocolate.Subscriptions;
using PushData.PushDataAPI.Models;
using System.Runtime.CompilerServices;

namespace PushData.PushDataAPI.GraphQL;

public class Subscription
{
  private readonly ITopicEventReceiver _eventReceiver;

  public Subscription(ITopicEventReceiver eventReceiver)
  {
    _eventReceiver = eventReceiver;
  }

  public async IAsyncEnumerable<Book> OnPublishedStream([EnumeratorCancellation] CancellationToken cancellationToken)
  {
    var sourceStream = await _eventReceiver.SubscribeAsync<Book>(nameof(GraphQL.Mutation.PublishBook), cancellationToken);

    await foreach (var book in sourceStream.ReadEventsAsync())
    {
      yield return book;
    }
  }

  [Subscribe(With = nameof(OnPublishedStream))]
  public Book OnPublished([EventMessage] Book publishBook) => publishBook;

  public async IAsyncEnumerable<string> RemoveBookStream([EnumeratorCancellation] CancellationToken cancellationToken)
  {
    var sourceStream = await _eventReceiver.SubscribeAsync<string>(nameof(GraphQL.Mutation.RemoveBook), cancellationToken);

    await foreach (var bookId in sourceStream.ReadEventsAsync())
    {
      yield return bookId;
    }
  }

  [Subscribe(With = nameof(RemoveBookStream))]
  public string RemoveBook([EventMessage] string bookId) => bookId;
}
