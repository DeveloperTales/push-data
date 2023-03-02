// eslint-disable-next-line @typescript-eslint/no-unused-vars
import {
  PushDataViews,
  MyApolloProvider,
  MyQueryClientProvider,
} from '@push-data/push-data-views';

export function App() {
  return (
    <>
      <MyApolloProvider>
        <MyQueryClientProvider>
          <PushDataViews />
        </MyQueryClientProvider>
      </MyApolloProvider>
      <div />
    </>
  );
}

export default App;
