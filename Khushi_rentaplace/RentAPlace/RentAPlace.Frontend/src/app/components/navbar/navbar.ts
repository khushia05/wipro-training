import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthService, User } from '../../services/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class NavbarComponent implements OnInit, OnDestroy {
  currentUser: User | null = null;
  showDropdown = false;
  showMobileMenu = false;
  private userSubscription?: Subscription;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.userSubscription = this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
  }

  ngOnDestroy() {
    if (this.userSubscription) {
      this.userSubscription.unsubscribe();
    }
  }

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  toggleDropdown(): void {
    this.showDropdown = !this.showDropdown;
  }

  closeDropdown(): void {
    this.showDropdown = false;
  }

  onProfileClick(): void {
    this.router.navigate(['/profile']);
    this.closeDropdown();
  }

  onBookingsClick(): void {
    this.router.navigate(['/bookings']);
    this.closeDropdown();
  }

  onFavoritesClick(): void {
    this.router.navigate(['/favorites']);
    this.closeDropdown();
  }

  onOwnerBookingsClick(): void {
    this.router.navigate(['/owner-bookings']);
    this.closeDropdown();
  }

  onMyPropertiesClick(): void {
    this.router.navigate(['/my-properties']);
    this.closeDropdown();
  }

  onLogoutClick(): void {
    this.authService.logout();
    this.closeDropdown();
  }

  onLoginClick(): void {
    this.router.navigate(['/login']);
  }

  onRegisterClick(): void {
    this.router.navigate(['/register']);
  }

  onLogoClick(): void {
    this.router.navigate(['/']);
  }

  toggleMobileMenu(): void {
    this.showMobileMenu = !this.showMobileMenu;
  }

  closeMobileMenu(): void {
    this.showMobileMenu = false;
  }
}
