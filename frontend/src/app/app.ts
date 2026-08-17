import { Component } from '@angular/core';
import { FeedPage } from './feed/feed-page/feed-page';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [FeedPage],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {}
