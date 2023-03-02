export const webApiHttpUrl = "https://localhost:7145/";
export const webApiWSUrl = 'wss://localhost:7145/';

export interface Author{
    name: string
}

export interface Book {
    id: string;
    title: string;
    author: Author;
  }