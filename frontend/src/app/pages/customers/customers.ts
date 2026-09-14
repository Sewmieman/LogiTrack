import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Api, Customer } from '../../services/api';
@Component({ selector: 'app-customers', standalone: true, imports: [FormsModule], templateUrl: './customers.html', styleUrl: './customers.css' })
export class Customers implements OnInit {
  private readonly api = inject(Api);
  customers: Customer[] = []; search = ''; showForm = false; loading = false; errorMessage = ''; successMessage = '';
  form = { firstName:'', lastName:'', phone:'', email:'', address:'' };
  ngOnInit(): void { this.load(); }
  load(): void { this.loading=true; this.api.getCustomers().subscribe({next:d=>{this.customers=d;this.loading=false;}, error:e=>{this.errorMessage=e.error?.message??'Could not load customers.';this.loading=false;}}); }
  filtered(): Customer[] { const q=this.search.toLowerCase(); return this.customers.filter(c => `${c.firstName} ${c.lastName} ${c.email} ${c.phone} ${c.address}`.toLowerCase().includes(q)); }
  save(): void { this.errorMessage=''; this.api.createCustomer(this.form).subscribe({next:c=>{this.customers=[...this.customers,c];this.form={firstName:'',lastName:'',phone:'',email:'',address:''};this.showForm=false;this.successMessage='Customer created successfully.';}, error:e=>this.errorMessage=e.error?.message??'Could not create customer.'}); }
}
