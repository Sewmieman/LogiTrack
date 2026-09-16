import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { Api, Delivery, DeliveryStatus } from '../../services/api';

@Component({ selector: 'app-dashboard', standalone: true, imports: [DatePipe], templateUrl: './dashboard.html', styleUrl: './dashboard.css' })
export class Dashboard implements OnInit {
  private readonly api = inject(Api);
  private readonly router = inject(Router);
  deliveries: Delivery[] = [];
  loading = true;
  errorMessage = '';
  readonly DeliveryStatus = DeliveryStatus;

  ngOnInit(): void { this.load(); }
  load(): void {
    this.loading = true;
    this.api.getDeliveries().subscribe({
      next: data => { this.deliveries = data; this.loading = false; },
      error: e => { this.errorMessage = e.error?.message ?? 'Could not load dashboard data.'; this.loading = false; }
    });
  }
  count(status: DeliveryStatus): number { return this.deliveries.filter(d => d.status === status).length; }
  recent(): Delivery[] { return [...this.deliveries].sort((a,b) => +new Date(b.createdAt) - +new Date(a.createdAt)).slice(0,5); }
  goDeliveries(): void { this.router.navigate(['/deliveries']); }
  statusName(status: DeliveryStatus): string { return DeliveryStatus[status] ?? String(status); }
}
