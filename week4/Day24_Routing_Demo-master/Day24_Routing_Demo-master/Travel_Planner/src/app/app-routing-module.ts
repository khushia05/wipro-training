import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// import { HomeComponent } from './home/home.component';
import { HomeComponent } from './home-component/home-component';
import { DestinationComponent } from './destination-component/destination-component';
// import { DestinationComponent } from './destinations/destinations.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: 'destinations',
    loadComponent: () =>
      import('./destination-component/destination-component')
        .then(m => m.DestinationComponent)
  },
  { path: '**', redirectTo: '' }
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }