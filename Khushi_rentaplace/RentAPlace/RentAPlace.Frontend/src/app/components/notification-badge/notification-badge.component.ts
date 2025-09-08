import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { NotificationService, Notification, NotificationStats } from '../../services/notification.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-notification-badge',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="notification-badge" (click)="toggleDropdown()">
      <div class="notification-icon">
        🔔
        <span class="badge" *ngIf="stats.unreadCount > 0">{{ stats.unreadCount }}</span>
      </div>
      
      <div class="dropdown" [class.show]="showDropdown" *ngIf="showDropdown">
        <div class="dropdown-header">
          <h3>Notifications</h3>
          <button class="close-btn" (click)="closeDropdown()">×</button>
        </div>
        
        <div class="notifications-list" *ngIf="notifications.length > 0; else noNotifications">
          <div 
            *ngFor="let notification of notifications" 
            class="notification-item"
            [class.unread]="!notification.isRead"
            (click)="markAsRead(notification)">
            <div class="notification-content">
              <h4>{{ notification.subject }}</h4>
              <p>{{ notification.body | slice:0:100 }}{{ notification.body.length > 100 ? '...' : '' }}</p>
              <small class="notification-time">{{ getTimeAgo(notification.createdAt) }}</small>
            </div>
            <div class="notification-actions">
              <button 
                class="delete-btn" 
                (click)="deleteNotification(notification.id, $event)"
                title="Delete notification">
                🗑️
              </button>
            </div>
          </div>
        </div>
        
        <ng-template #noNotifications>
          <div class="no-notifications">
            <p>No notifications yet</p>
          </div>
        </ng-template>
        
        <div class="dropdown-footer">
          <button class="view-all-btn" (click)="viewAllNotifications()">
            View All Notifications
          </button>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .notification-badge {
      position: relative;
      display: inline-block;
    }

    .notification-icon {
      position: relative;
      cursor: pointer;
      font-size: 1.5rem;
      padding: 8px;
      border-radius: 50%;
      transition: background-color 0.3s ease;
    }

    .notification-icon:hover {
      background-color: #f0f0f0;
    }

    .badge {
      position: absolute;
      top: 0;
      right: 0;
      background: #e74c3c;
      color: white;
      border-radius: 50%;
      width: 20px;
      height: 20px;
      font-size: 0.7rem;
      display: flex;
      align-items: center;
      justify-content: center;
      font-weight: bold;
    }

    .dropdown {
      position: absolute;
      top: 100%;
      right: 0;
      background: white;
      border: 1px solid #ddd;
      border-radius: 8px;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
      width: 350px;
      max-height: 400px;
      overflow: hidden;
      z-index: 1000;
    }

    .dropdown-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 15px;
      border-bottom: 1px solid #eee;
      background: #f8f9fa;
    }

    .dropdown-header h3 {
      margin: 0;
      font-size: 1.1rem;
      color: #333;
    }

    .close-btn {
      background: none;
      border: none;
      font-size: 1.2rem;
      cursor: pointer;
      color: #666;
      padding: 0;
      width: 24px;
      height: 24px;
      display: flex;
      align-items: center;
      justify-content: center;
    }

    .notifications-list {
      max-height: 300px;
      overflow-y: auto;
    }

    .notification-item {
      display: flex;
      padding: 12px 15px;
      border-bottom: 1px solid #f0f0f0;
      cursor: pointer;
      transition: background-color 0.2s ease;
    }

    .notification-item:hover {
      background-color: #f8f9fa;
    }

    .notification-item.unread {
      background-color: #e3f2fd;
      border-left: 3px solid #2196f3;
    }

    .notification-content {
      flex: 1;
    }

    .notification-content h4 {
      margin: 0 0 4px 0;
      font-size: 0.9rem;
      color: #333;
      font-weight: 600;
    }

    .notification-content p {
      margin: 0 0 4px 0;
      font-size: 0.8rem;
      color: #666;
      line-height: 1.4;
    }

    .notification-time {
      color: #999;
      font-size: 0.7rem;
    }

    .notification-actions {
      display: flex;
      align-items: center;
      margin-left: 8px;
    }

    .delete-btn {
      background: none;
      border: none;
      cursor: pointer;
      font-size: 0.8rem;
      color: #999;
      padding: 4px;
      border-radius: 3px;
      transition: all 0.2s ease;
    }

    .delete-btn:hover {
      background-color: #ffebee;
      color: #e74c3c;
    }

    .no-notifications {
      padding: 20px;
      text-align: center;
      color: #666;
    }

    .dropdown-footer {
      padding: 10px 15px;
      border-top: 1px solid #eee;
      background: #f8f9fa;
    }

    .view-all-btn {
      width: 100%;
      background: #667eea;
      color: white;
      border: none;
      padding: 8px 12px;
      border-radius: 4px;
      cursor: pointer;
      font-size: 0.9rem;
      transition: background-color 0.2s ease;
    }

    .view-all-btn:hover {
      background: #5a6fd8;
    }

    @media (max-width: 768px) {
      .dropdown {
        width: 300px;
        right: -50px;
      }
    }
  `]
})
export class NotificationBadgeComponent implements OnInit, OnDestroy {
  @Input() maxNotifications = 5;
  
  notifications: Notification[] = [];
  stats: NotificationStats = {
    totalNotifications: 0,
    unreadCount: 0,
    pendingCount: 0,
    failedCount: 0
  };
  
  showDropdown = false;
  private subscription = new Subscription();

  constructor(
    private notificationService: NotificationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadNotifications();
    this.loadStats();
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  loadNotifications(): void {
    this.subscription.add(
      this.notificationService.getNotifications({
        page: 1,
        pageSize: this.maxNotifications
      }).subscribe(notifications => {
        this.notifications = notifications;
      })
    );
  }

  loadStats(): void {
    this.subscription.add(
      this.notificationService.getNotificationStats().subscribe(stats => {
        this.stats = stats;
      })
    );
  }

  toggleDropdown(): void {
    this.showDropdown = !this.showDropdown;
    if (this.showDropdown) {
      this.loadNotifications();
    }
  }

  closeDropdown(): void {
    this.showDropdown = false;
  }

  markAsRead(notification: Notification): void {
    if (!notification.isRead) {
      this.subscription.add(
        this.notificationService.markAsRead(notification.id).subscribe(() => {
          notification.isRead = true;
          this.loadStats();
        })
      );
    }
  }

  deleteNotification(notificationId: number, event: Event): void {
    event.stopPropagation();
    
    if (confirm('Are you sure you want to delete this notification?')) {
      this.subscription.add(
        this.notificationService.deleteNotification(notificationId).subscribe(() => {
          this.notifications = this.notifications.filter(n => n.id !== notificationId);
          this.loadStats();
        })
      );
    }
  }

  viewAllNotifications(): void {
    this.closeDropdown();
    this.router.navigate(['/notifications']);
  }

  getTimeAgo(dateString: string): string {
    const date = new Date(dateString);
    const now = new Date();
    const diffInSeconds = Math.floor((now.getTime() - date.getTime()) / 1000);

    if (diffInSeconds < 60) {
      return 'Just now';
    } else if (diffInSeconds < 3600) {
      const minutes = Math.floor(diffInSeconds / 60);
      return `${minutes}m ago`;
    } else if (diffInSeconds < 86400) {
      const hours = Math.floor(diffInSeconds / 3600);
      return `${hours}h ago`;
    } else {
      const days = Math.floor(diffInSeconds / 86400);
      return `${days}d ago`;
    }
  }
}
