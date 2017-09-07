export class VisitListConfig {
  type = 'all';

  filters: {
    limit?: number,
    offset?: number,
    username?: string
  } = {};
}