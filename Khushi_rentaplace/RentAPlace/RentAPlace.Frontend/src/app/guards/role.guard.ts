import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    const user = this.authService.getCurrentUser();
    
    if (!user) {
      this.router.navigate(['/login']);
      return false;
    }

    const expectedRole = route.data['role'] as string;
    
    if (user.userType === expectedRole) {
      return true;
    }

    // Redirect to appropriate dashboard based on user role
    if (user.userType === 'User') {
      this.router.navigate(['/user-dashboard']);
    } else if (user.userType === 'Owner') {
      this.router.navigate(['/owner-dashboard']);
    } else {
      this.router.navigate(['/']);
    }
    
    return false;
  }
}
