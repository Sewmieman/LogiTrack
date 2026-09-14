import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export enum DeliveryStatus { Pending = 1, Assigned = 2, PickedUp = 3, InTransit = 4, Delivered = 5, Cancelled = 6, Failed = 7 }
export enum DriverStatus { Available = 1, Busy = 2, Offline = 3, Suspended = 4 }
export enum VehicleStatus { Available = 1, InUse = 2, Maintenance = 3, Retired = 4 }
export enum PaymentStatus { Pending = 1, Paid = 2, Failed = 3, Refunded = 4 }

export interface Customer { id: number; firstName: string; lastName: string; phone: string; email: string; address: string; createdAt: string; }
export interface Driver { id: number; firstName: string; lastName: string; phone: string; licenseNumber: string; status: DriverStatus; }
export interface Vehicle { id: number; plateNumber: string; model: string; type: string; year: number; capacityKg: number; status: VehicleStatus; }
export interface Delivery { id: number; trackingNumber: string; customerId: number; driverId: number | null; vehicleId: number | null; pickupAddress: string; deliveryAddress: string; packageDescription: string; weightKg: number; status: DeliveryStatus; deliveryFee: number; expectedDeliveryDate: string | null; deliveredAt: string | null; createdAt: string; }
export interface DeliveryTracking { id: number; deliveryId: number; status: string; location: string; notes: string | null; createdAt: string; }
export interface Payment { id: number; deliveryId: number; amount: number; paymentMethod: string; status: PaymentStatus; paidAt: string | null; createdAt: string; }

export type CreateCustomer = Omit<Customer, 'id' | 'createdAt'>;
export type CreateDriver = Omit<Driver, 'id' | 'status'>;
export type UpdateDriver = Omit<Driver, never>;
export type CreateVehicle = Omit<Vehicle, 'id' | 'status'>;
export type UpdateVehicle = Vehicle;
export interface CreateDelivery { customerId: number; pickupAddress: string; deliveryAddress: string; packageDescription: string; weightKg: number; deliveryFee: number; expectedDeliveryDate: string | null; }
export interface AssignDelivery { deliveryId: number; driverId: number; vehicleId: number; }
export interface UpdateDeliveryStatus { deliveryId: number; status: DeliveryStatus; location: string; notes: string | null; }
export interface CreatePayment { deliveryId: number; amount: number; paymentMethod: string; }

@Injectable({ providedIn: 'root' })
export class Api {
  private readonly http = inject(HttpClient);
  readonly baseUrl = 'http://localhost:5148/api';

  getCustomers(): Observable<Customer[]> { return this.http.get<Customer[]>(`${this.baseUrl}/customers`); }
  getCustomer(id: number): Observable<Customer> { return this.http.get<Customer>(`${this.baseUrl}/customers/${id}`); }
  createCustomer(body: CreateCustomer): Observable<Customer> { return this.http.post<Customer>(`${this.baseUrl}/customers`, body); }

  getDrivers(): Observable<Driver[]> { return this.http.get<Driver[]>(`${this.baseUrl}/drivers`); }
  createDriver(body: CreateDriver): Observable<Driver> { return this.http.post<Driver>(`${this.baseUrl}/drivers`, body); }
  updateDriver(body: UpdateDriver): Observable<Driver> { return this.http.put<Driver>(`${this.baseUrl}/drivers/${body.id}`, body); }
  deleteDriver(id: number): Observable<void> { return this.http.delete<void>(`${this.baseUrl}/drivers/${id}`); }

  getVehicles(): Observable<Vehicle[]> { return this.http.get<Vehicle[]>(`${this.baseUrl}/vehicles`); }
  createVehicle(body: CreateVehicle): Observable<Vehicle> { return this.http.post<Vehicle>(`${this.baseUrl}/vehicles`, body); }
  updateVehicle(body: UpdateVehicle): Observable<Vehicle> { return this.http.put<Vehicle>(`${this.baseUrl}/vehicles/${body.id}`, body); }
  deleteVehicle(id: number): Observable<void> { return this.http.delete<void>(`${this.baseUrl}/vehicles/${id}`); }

  getDeliveries(): Observable<Delivery[]> { return this.http.get<Delivery[]>(`${this.baseUrl}/deliveries`); }
  createDelivery(body: CreateDelivery): Observable<Delivery> { return this.http.post<Delivery>(`${this.baseUrl}/deliveries`, body); }
  assignDelivery(body: AssignDelivery): Observable<Delivery> { return this.http.post<Delivery>(`${this.baseUrl}/deliveries/${body.deliveryId}/assign`, body); }
  updateDeliveryStatus(body: UpdateDeliveryStatus): Observable<Delivery> { return this.http.put<Delivery>(`${this.baseUrl}/deliveries/${body.deliveryId}/status`, body); }
  getDeliveryTracking(id: number): Observable<DeliveryTracking[]> { return this.http.get<DeliveryTracking[]>(`${this.baseUrl}/deliveries/${id}/tracking`); }

  getPaymentsByDelivery(deliveryId: number): Observable<Payment[]> { return this.http.get<Payment[]>(`${this.baseUrl}/payments/delivery/${deliveryId}`); }
  createPayment(body: CreatePayment): Observable<Payment> { return this.http.post<Payment>(`${this.baseUrl}/payments`, body); }
  payPayment(id: number): Observable<Payment> { return this.http.put<Payment>(`${this.baseUrl}/payments/${id}/pay`, {}); }
}
