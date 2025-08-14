import { Component, signal } from '@angular/core';
import { RouterModule } from '@angular/router';
import { RouterLink } from '@angular/router';
@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css',
  providers:[RouterLink]
})
export class App {
  protected readonly title = signal('Travel_Planner');
}

