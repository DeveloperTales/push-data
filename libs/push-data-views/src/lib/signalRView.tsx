import { useQuery } from 'react-query';
import { useEffect, useRef, useState } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { GraphQLClient, gql } from 'graphql-request';
import { webApiHttpUrl } from '../api/apiValues';
import { BooksList } from '../components/bookList';

const signalRConnection = new HubConnectionBuilder()
  .withUrl(`${webApiHttpUrl}bookHub`)
  .withAutomaticReconnect()
  .build();

const graphQLClient = new GraphQLClient(`${webApiHttpUrl}graphql/`, {
  headers: {
    Authorization: `Bearer ${process.env.API_KEY}`,
  },
});

interface Author {
  name: string;
}

interface Book {
  id: string;
  title: string;
  author: Author;
}

const BOOKS_QUERY = gql`
  {
    books {
      id
      title
      author {
        name
      }
    }
  }
`;

/* eslint-disable-next-line */
export interface TestSignalRProps {}

export function TestSignalR(props: TestSignalRProps) {
  const { data, error, isLoading } = useQuery('books', async () => {
    const { books } = await graphQLClient.request<{books: Book[]}>(BOOKS_QUERY);
    return books;
  });

  const [books, setBooks] = useState<Book[]>([]);
  const [isConnectionSet, setIsConnectionSet] = useState<boolean>(false);
  const latestBooks = useRef<Book[]>([]);
  latestBooks.current = books;

  useEffect(() => {
    if (signalRConnection && !isConnectionSet) {
      signalRConnection
        .start()
        .then((result) => {
          console.log('Connected!');
          signalRConnection.on('PublishBook', (book) => {
            addBook(book);
          });
          signalRConnection.on('RemoveBook', (bookId) => {
            removeBook(bookId);
          });
          setIsConnectionSet(true);
        })
        .catch((e) => console.error('Connection failed: ', e));
    }
  }, [isConnectionSet, setIsConnectionSet]);

  useEffect(() => {
    if (!isLoading && data) {
      setBooks(data);
    }
  }, [isLoading, data, setBooks]);

  const addBook = (book: Book) => {
    const updatedBooks = [...latestBooks.current];
    console.log('addBook', book);
    if (!updatedBooks.some((b) => b.id === book.id)) {
      updatedBooks.push(book);
      setBooks(updatedBooks);
    }
  };

  const removeBook = (bookId: string) => {
    const updatedBooks = [...latestBooks.current];
    console.log('removeBook', bookId);
    const index = updatedBooks.findIndex((b) => b.id === bookId);
    if (index !== undefined) {
      updatedBooks.splice(index, 1);
      setBooks(updatedBooks);
    }
  };

  if (isLoading) return <div>Loading...</div>;
  if (error) return <pre>"error"</pre>;

  return (
    <div>
      <BooksList tableName="SignalR" books={books} />
    </div>
  );
}
