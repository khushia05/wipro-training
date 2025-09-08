import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home';
import { CategoryComponent } from './pages/category/category';
import { CategoryPropertiesComponent } from './pages/category-properties/category-properties';
import { SearchResultsComponent } from './pages/search-results/search-results';
import { ApartmentDetailsComponent } from './pages/apartment-details/apartment-details';
import { LoginComponent } from './pages/login/login';
import { RegisterComponent } from './pages/register/register';
import { ProfileComponent } from './pages/profile/profile';
import { BookingsComponent } from './pages/bookings/bookings';
import { FavoritesComponent } from './pages/favorites/favorites';
import { OwnerBookingsComponent } from './pages/owner-bookings/owner-bookings';
import { AddPropertyComponent } from './pages/add-property/add-property';
import { MyPropertiesComponent } from './pages/my-properties/my-properties';
import { UserDashboardComponent } from './pages/user-dashboard/user-dashboard';
import { OwnerDashboardComponent } from './pages/owner-dashboard/owner-dashboard';
import { AuthGuard } from './guards/auth.guard';
import { RoleGuard } from './guards/role.guard';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { 
    path: 'properties/category/:categoryName', 
    component: CategoryComponent
  },
  { 
    path: 'properties/:category', 
    component: CategoryPropertiesComponent
  },
  { 
    path: 'search', 
    component: SearchResultsComponent
  },
  { 
    path: 'property/:id', 
    component: ApartmentDetailsComponent
  },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { 
    path: 'profile', 
    component: ProfileComponent,
    canActivate: [AuthGuard]
  },
  { 
    path: 'bookings', 
    component: BookingsComponent,
    canActivate: [AuthGuard]
  },
  { 
    path: 'favorites', 
    component: FavoritesComponent,
    canActivate: [AuthGuard]
  },
  { 
    path: 'owner-bookings', 
    component: OwnerBookingsComponent,
    canActivate: [RoleGuard],
    data: { role: 'Owner' }
  },
  { 
    path: 'add-property', 
    component: AddPropertyComponent,
    canActivate: [RoleGuard],
    data: { role: 'Owner' }
  },
  { 
    path: 'my-properties', 
    component: MyPropertiesComponent,
    canActivate: [RoleGuard],
    data: { role: 'Owner' }
  },
  { 
    path: 'user-dashboard', 
    component: UserDashboardComponent,
    canActivate: [RoleGuard],
    data: { role: 'User' }
  },
  { 
    path: 'owner-dashboard', 
    component: OwnerDashboardComponent,
    canActivate: [RoleGuard],
    data: { role: 'Owner' }
  },
  { path: '**', redirectTo: '' }
];