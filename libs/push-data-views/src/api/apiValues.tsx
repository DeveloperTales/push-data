export const webApiHttpUrl = "http://localhost:5177/";
export const webApiWSUrl = 'ws://localhost:5177/';

export interface Author{
    name: string
}

export interface Book {
    id: string;
    title: string;
    author: Author;
  }