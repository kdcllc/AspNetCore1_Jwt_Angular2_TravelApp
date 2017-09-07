export class LocationListConfig{
      state = 'all';
      username?:string;

  filters: {
    limit?: number,
    offset?: number,
  } = {};
}