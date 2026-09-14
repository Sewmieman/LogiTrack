import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { Api, Customer, Delivery, DeliveryStatus, DeliveryTracking, Driver, DriverStatus, Vehicle, VehicleStatus } from '../../services/api';

@Component({ selector:'app-deliveries', standalone:true, imports:[FormsModule, DatePipe], templateUrl:'./deliveries.html', styleUrl:'./deliveries.css' })
export class Deliveries implements OnInit {
  private readonly api = inject(Api);
  deliveries: Delivery[]=[]; customers:Customer[]=[]; drivers:Driver[]=[]; vehicles:Vehicle[]=[]; tracking:DeliveryTracking[]=[];
  search=''; statusFilter=0; loading=false; errorMessage=''; successMessage=''; showCreate=false; selected:Delivery|null=null; showTracking=false;
  statuses=[DeliveryStatus.Pending,DeliveryStatus.Assigned,DeliveryStatus.PickedUp,DeliveryStatus.InTransit,DeliveryStatus.Delivered,DeliveryStatus.Cancelled,DeliveryStatus.Failed];
  createForm={customerId:0,pickupAddress:'',deliveryAddress:'',packageDescription:'',weightKg:0,deliveryFee:0,expectedDeliveryDate:''};
  assignForm={driverId:0,vehicleId:0}; statusForm={status:DeliveryStatus.InTransit,location:'',notes:''};
  ngOnInit(){this.loadAll();}
  loadAll(){this.loading=true; this.errorMessage=''; this.api.getDeliveries().subscribe({next:d=>{this.deliveries=d;this.loading=false;},error:e=>{this.errorMessage=this.err(e,'Could not load deliveries.');this.loading=false;}}); this.api.getCustomers().subscribe({next:d=>this.customers=d}); this.api.getDrivers().subscribe({next:d=>this.drivers=d}); this.api.getVehicles().subscribe({next:d=>this.vehicles=d});}
  filtered(){const q=this.search.toLowerCase(); return this.deliveries.filter(d=>(!this.statusFilter||d.status===this.statusFilter)&&`${d.trackingNumber} ${d.packageDescription} ${d.pickupAddress} ${d.deliveryAddress}`.toLowerCase().includes(q));}
  statusName(s:number){return DeliveryStatus[s]??String(s);} customerName(id:number){const c=this.customers.find(x=>x.id===id);return c?`${c.firstName} ${c.lastName}`:`#${id}`;} driverName(id:number|null){const d=this.drivers.find(x=>x.id===id);return d?`${d.firstName} ${d.lastName}`:'Unassigned';} vehicleName(id:number|null){const v=this.vehicles.find(x=>x.id===id);return v?`${v.plateNumber} - ${v.model}`:'Unassigned';}
  availableDrivers(){return this.drivers.filter(d=>d.status===DriverStatus.Available || d.id===this.selected?.driverId);} availableVehicles(){return this.vehicles.filter(v=>v.status===VehicleStatus.Available || v.id===this.selected?.vehicleId);}
  create(){this.errorMessage=''; if(!this.createForm.customerId){this.errorMessage='Please select a customer.';return;} const body={...this.createForm,expectedDeliveryDate:this.createForm.expectedDeliveryDate||null}; this.api.createDelivery(body).subscribe({next:()=>{this.showCreate=false;this.successMessage='Delivery created successfully.';this.createForm={customerId:0,pickupAddress:'',deliveryAddress:'',packageDescription:'',weightKg:0,deliveryFee:0,expectedDeliveryDate:''};this.loadAll();},error:e=>this.errorMessage=this.err(e,'Could not create delivery.')});}
  select(d:Delivery){this.selected=d;this.assignForm={driverId:d.driverId??0,vehicleId:d.vehicleId??0};this.statusForm={status:d.status,location:'',notes:''};}
  assign(){if(!this.selected||!this.assignForm.driverId||!this.assignForm.vehicleId)return; this.api.assignDelivery({deliveryId:this.selected.id,...this.assignForm}).subscribe({next:d=>{this.selected=d;this.successMessage='Delivery assigned.';this.loadAll();},error:e=>this.errorMessage=this.err(e,'Could not assign delivery.')});}
  updateStatus(){if(!this.selected)return; this.api.updateDeliveryStatus({deliveryId:this.selected.id,status:this.statusForm.status,location:this.statusForm.location,notes:this.statusForm.notes||null}).subscribe({next:d=>{this.selected=d;this.successMessage='Delivery status updated.';this.loadAll();},error:e=>this.errorMessage=this.err(e,'Could not update status.')});}
  viewTracking(d:Delivery){this.selected=d;this.showTracking=true;this.tracking=[];this.api.getDeliveryTracking(d.id).subscribe({next:t=>this.tracking=t,error:e=>this.errorMessage=this.err(e,'Could not load tracking history.')});}
  private err(e:any,fallback:string){if(typeof e?.error==='string')return e.error; return e?.error?.message??fallback;}
}
