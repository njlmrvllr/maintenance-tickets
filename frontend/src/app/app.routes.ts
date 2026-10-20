import { Routes } from '@angular/router';

const maintenanceRoutes = () =>
  import('./maintenance/presentation/maintenance.routes').then((m) => m.maintenanceRoutes);
const pageNotFound = () =>
  import('./shared/presentation/views/page-not-found/page-not-found').then((m) => m.PageNotFound);

export const routes: Routes = [
  { path: '', redirectTo: 'report', pathMatch: 'full' },
  { path: '', loadChildren: maintenanceRoutes },
  { path: '**', loadComponent: pageNotFound, title: 'Page not found · Maintenance Tickets' },
];