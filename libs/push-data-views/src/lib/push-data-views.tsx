import { Box, Grid, Typography } from '@mui/material';
import Button from '@mui/material/Button';
import Stack from '@mui/material/Stack';
import Toolbar from '@mui/material/Toolbar';
import { TestSignalR } from './signalRView';
import { Subscription } from './subscribtionView';
import { webApiHttpUrl } from '../api/apiValues';


/* eslint-disable-next-line */
export interface PushDataViewsProps {}

export function PushDataViews(props: PushDataViewsProps) {

  const addRandomBook = () => {
    const url = new URL(webApiHttpUrl);
    url.pathname = '/api/BooksRandom';
    fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
    });
  };

  const removeRandomBook = () => {
    const url = new URL(webApiHttpUrl);
    url.pathname = '/api/BooksRandom';
    fetch(url, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
      },
    });
  };

  return (
    <Box sx={{ width: '100%' }}>
      <Toolbar>
        <Typography variant="h4">Pushing data from api to UI</Typography>
      </Toolbar>
      <Toolbar>
        <Stack spacing={2} direction="row">
          <Button variant="contained" onClick={addRandomBook}>
            Add Random Book
          </Button>
          <Button variant="outlined" color="error" onClick={removeRandomBook}>
            Remove Random Book
          </Button>
        </Stack>
      </Toolbar>
      <Grid container spacing={2}>
        <Grid item xs={6}>
          <Subscription />
        </Grid>
        <Grid item xs={6}>
          <TestSignalR />
        </Grid>
      </Grid>
    </Box>
  );
}

export default PushDataViews;
