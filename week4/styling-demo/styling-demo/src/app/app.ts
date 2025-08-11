import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { StyledCard } from './styled-card/styled-card';
@Component({
  selector: 'app-root',
  imports: [RouterOutlet,StyledCard],
  templateUrl: './app.html',
  styleUrls: ['./app.css'],
  
    styles:[
    `.Inline-css {color:darkblue;}`//inline 
  ]
  
})
export class App {
  protected readonly title = signal('styling-demo');
}
