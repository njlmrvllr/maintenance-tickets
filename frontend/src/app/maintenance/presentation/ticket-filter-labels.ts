import { CreatedWithin } from '../domain/model/ticket-filter';

export const CREATED_WITHIN_LABELS: Record<CreatedWithin, string> = {
  any: 'Any date',
  today: 'Today',
  last7Days: 'Last 7 days',
  last30Days: 'Last 30 days',
};
