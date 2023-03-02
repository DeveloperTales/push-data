import { useQuery, gql, useSubscription } from '@apollo/client';
import { useState, useEffect, useRef } from 'react';
import { BooksList } from '../components/bookList';

interface Author {
  name: string;
}

interface Book {
  id: string;
  title: string;
  author: Author;
}

const BOOKS_SUB = gql`
  subscription onPublished {
    onPublished {
      id
      title
      author {
        name
      }
    }
  }
`;

const BOOK_REMOVED_SUB = gql`
  subscription removeBook {
    removeBook
  }
`;

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
export interface SubscriptionProps {}

export function Subscription(props: SubscriptionProps) {
  const { data, loading, error } = useQuery(BOOKS_QUERY);
  const { data: data2, loading: loading2 } = useSubscription(BOOKS_SUB);
  const { data: data3, loading: loading3 } = useSubscription(BOOK_REMOVED_SUB);

  const [books, setBooks] = useState<Book[]>([]);
  const latestBooks = useRef<Book[]>([]);
  latestBooks.current = books;

  useEffect(() => {
    if (!loading && data) {
      console.warn('setBooks');
      setBooks(data['books']);
    }
  }, [loading, data, setBooks]);

  useEffect(() => {
    if (!loading) {
      if (!loading2 && data2) {
        addBook(data2.onPublished);
      }
    }
  }, [loading, loading2, data2]);

  useEffect(() => {
    if (!loading) {
      if (!loading3 && data3) {
        removeBook(data3.removeBook);
      }
    }
  }, [loading, loading3, data3]);

  if (loading) return <div>Loading...</div>;
  if (error) return <pre>{error.message}</pre>;

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

  return <BooksList tableName="GraphQL Subscriptions" books={books} />;
}
