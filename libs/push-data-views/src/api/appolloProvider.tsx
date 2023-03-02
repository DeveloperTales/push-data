import { ApolloProvider, ApolloClient, InMemoryCache } from "@apollo/client";
import { GraphQLWsLink } from '@apollo/client/link/subscriptions';
import { createClient } from 'graphql-ws';
import { split, HttpLink } from '@apollo/client';
import { getMainDefinition } from '@apollo/client/utilities';
import { webApiWSUrl, webApiHttpUrl } from "./apiValues";

const wsLink = new GraphQLWsLink(createClient({
  url: `${webApiWSUrl}graphql/`,
}));

const httpLink = new HttpLink({
  uri: `${webApiHttpUrl}graphql/`
});

// The split function takes three parameters:
//
// * A function that's called for each operation to execute
// * The Link to use for an operation if the function returns a "truthy" value
// * The Link to use for an operation if the function returns a "falsy" value
const splitLink = split(
  ({ query }) => {
    const definition = getMainDefinition(query);
    return (
      definition.kind === 'OperationDefinition' &&
      definition.operation === 'subscription'
    );
  },
  wsLink,
  httpLink,
);

const client = new ApolloClient({
  link: splitLink,
  cache: new InMemoryCache(),  
});

interface Props {
  children?: React.ReactNode;
}

export function MyApolloProvider({ children }: Props) {
  return (
    <ApolloProvider client={client}>
      {children}
    </ApolloProvider>
  )
}
