import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { forkJoin, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Api, Delivery, Payment, PaymentStatus } from '../../services/api';
@Component({selector:'app-payments',standalone:true,imports:[FormsModule,DatePipe],templateUrl:'./payments.html',styleUrl:'./payments.css'})
export class Payments implements OnInit{
 private readonly api=inject(Api); deliveries:Delivery[]=[]; payments:Payment[]=[]; search='';statusFilter=0;showForm=false;errorMessage='';successMessage='';statuses=[1,2,3,4];form={deliveryId:0,amount:0,paymentMethod:'Cash'};
 ngOnInit(){this.load();}
 load(){this.api.getDeliveries().subscribe({next:ds=>{this.deliveries=ds;if(ds.length===0){this.payments=[];return;} forkJoin(ds.map(d=>this.api.getPaymentsByDelivery(d.id).pipe(catchError(()=>of([] as Payment[]))))).subscribe(groups=>this.payments=groups.flat());},error:e=>this.errorMessage=e.error?.message??'Could not load payments.'});}
 filtered(){const q=this.search.toLowerCase();return this.payments.filter(p=>(!this.statusFilter||p.status===this.statusFilter)&&`${this.tracking(p.deliveryId)} ${p.paymentMethod} ${p.amount}`.toLowerCase().includes(q));}
 statusName(s:number){return PaymentStatus[s]??String(s);} tracking(id:number){return this.deliveries.find(d=>d.id===id)?.trackingNumber??`Delivery #${id}`;}
 create(){if(!this.form.deliveryId)return;this.api.createPayment(this.form).subscribe({next:()=>{this.showForm=false;this.successMessage='Payment created.';this.form={deliveryId:0,amount:0,paymentMethod:'Cash'};this.load();},error:e=>this.errorMessage=e.error?.message??'Could not create payment.'});}
 markPaid(id:number){this.api.payPayment(id).subscribe({next:()=>{this.successMessage='Payment marked as paid.';this.load();},error:e=>this.errorMessage=e.error?.message??'Could not update payment.'});}
}
