import { bootstrapApplication, platformBrowser } from '@angular/platform-browser';
import { AppModule } from './app/app-module';
import { Routes } from '@angular/router';
import { routes } from './app/app-routing-module';
import { App } from './app/app';
import { provideRouter } from '@angular/router';
// import 
platformBrowser().bootstrapModule(AppModule, {
  ngZoneEventCoalescing: true,
})
  .catch(err => console.error(err));
// bootstrapApplication(APPCOMP)
bootstrapApplication(App,
  {providers:[provideRouter(routes)]}
)