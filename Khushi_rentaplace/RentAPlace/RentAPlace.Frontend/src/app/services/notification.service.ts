import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject, timer, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

export interface Notification {
  id: number;
  recipientEmail: string;
  subject: string;
  body: string;
  type: string;
  relatedEntityId?: number;
  status: string;
  retryCount: number;
  errorMessage?: string;
  createdAt: string;
  sentAt?: string;
  lastRetryAt?: string;
  nextRetryAt?: string;
  isRead: boolean;
}

export interface NotificationStats {
  totalNotifications: number;
  unreadCount: number;
  pendingCount: number;
  failedCount: number;
}

export interface NotificationFilters {
  page?: number;
  pageSize?: number;
  status?: string;
  type?: string;
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private apiUrl = 'http://localhost:5158/api/notifications';
  private statsSubject = new BehaviorSubject<NotificationStats>({
    totalNotifications: 0,
    unreadCount: 0,
    pendingCount: 0,
    failedCount: 0
  });
  
  public stats$ = this.statsSubject.asObservable();
  private pollingInterval = 30000; // 30 seconds
  private pollingTimer?: any;

  constructor(private http: HttpClient) {
    this.startPolling();
  }

  getNotifications(filters: NotificationFilters = {}): Observable<Notification[]> {
    let params = new HttpParams();
    
    if (filters.page) params = params.set('page', filters.page.toString());
    if (filters.pageSize) params = params.set('pageSize', filters.pageSize.toString());
    if (filters.status) params = params.set('status', filters.status);
    if (filters.type) params = params.set('type', filters.type);

    return this.http.get<Notification[]>(this.apiUrl, { params }).pipe(
      catchError(error => {
        console.error('Error fetching notifications:', error);
        return of([]); // Observable return
      })
    );
  }

  getNotificationStats(): Observable<NotificationStats> {
    return this.http.get<NotificationStats>(`${this.apiUrl}/stats`).pipe(
      map(stats => {
        this.statsSubject.next(stats);
        return stats;
      }),
      catchError(error => {
        console.error('Error fetching notification stats:', error);
        return of(this.statsSubject.value); // Observable return
      })
    );
  }

  markAsRead(notificationId: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/${notificationId}/mark-read`, {}).pipe(
      map(() => {
        const currentStats = this.statsSubject.value;
        if (currentStats.unreadCount > 0) {
          this.statsSubject.next({
            ...currentStats,
            unreadCount: currentStats.unreadCount - 1
          });
        }
      }),
      catchError(error => {
        console.error('Error marking notification as read:', error);
        throw error;
      })
    );
  }

  deleteNotification(notificationId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${notificationId}`).pipe(
      map(() => {
        const currentStats = this.statsSubject.value;
        this.statsSubject.next({
          ...currentStats,
          totalNotifications: Math.max(0, currentStats.totalNotifications - 1),
          unreadCount: Math.max(0, currentStats.unreadCount - 1)
        });
      }),
      catchError(error => {
        console.error('Error deleting notification:', error);
        throw error;
      })
    );
  }

  private startPolling(): void {
    this.getNotificationStats().subscribe();

    this.pollingTimer = timer(this.pollingInterval, this.pollingInterval).subscribe(() => {
      this.getNotificationStats().subscribe();
    });
  }

  stopPolling(): void {
    if (this.pollingTimer) {
      this.pollingTimer.unsubscribe();
      this.pollingTimer = undefined;
    }
  }

  ngOnDestroy(): void {
    this.stopPolling();
  }
}
