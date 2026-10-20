import { Routes } from '@angular/router';

const reportIncident = () =>
  import('./views/report-incident/report-incident').then((m) => m.ReportIncident);
const operatorBoard = () =>
  import('./views/operator-board/operator-board').then((m) => m.OperatorBoard);

/**
 * Routes of the Maintenance bounded context: one view per role.
 */
export const maintenanceRoutes: Routes = [
  { path: 'report', loadComponent: reportIncident, title: 'Report an incident · Maintenance Tickets' },
  { path: 'board', loadComponent: operatorBoard, title: 'Operator board · Maintenance Tickets' },
];