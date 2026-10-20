import { Component } from '@angular/core';
import { NotificationCenter } from './maintenance/presentation/components/notification-center/notification-center';
import { Layout } from './shared/presentation/components/layout/layout';

@Component({
  selector: 'app-root',
  imports: [Layout, NotificationCenter],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {}
