import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Api, Driver, DriverStatus } from '../../services/api';
@Component({ selector:'app-drivers', standalone:true, imports:[FormsModule], templateUrl:'./drivers.html', styleUrl:'./drivers.css' })
export class Drivers implements OnInit {
 private readonly api=inject(Api); drivers:Driver[]=[]; search=''; showForm=false; editing:Driver|null=null; errorMessage='';
 form={firstName:'',lastName:'',phone:'',licenseNumber:'',status:DriverStatus.Available}; statuses=[1,2,3,4];
 ngOnInit(){this.load();} load(){this.api.getDrivers().subscribe({next:d=>this.drivers=d,error:e=>this.errorMessage=e.error?.message??'Could not load drivers.'});}
 filtered(){const q=this.search.toLowerCase();return this.drivers.filter(d=>`${d.firstName} ${d.lastName} ${d.phone} ${d.licenseNumber}`.toLowerCase().includes(q));}
 statusName(s:number){return DriverStatus[s]??String(s);} openNew(){this.editing=null;this.form={firstName:'',lastName:'',phone:'',licenseNumber:'',status:DriverStatus.Available};this.showForm=true;}
 edit(d:Driver){this.editing=d;this.form={firstName:d.firstName,lastName:d.lastName,phone:d.phone,licenseNumber:d.licenseNumber,status:d.status};this.showForm=true;}
 save(){const req=this.editing?this.api.updateDriver({id:this.editing.id,...this.form}):this.api.createDriver({firstName:this.form.firstName,lastName:this.form.lastName,phone:this.form.phone,licenseNumber:this.form.licenseNumber});req.subscribe({next:()=>{this.showForm=false;this.load();},error:e=>this.errorMessage=e.error?.message??'Could not save driver.'});}
 remove(id:number){if(confirm('Delete this driver?'))this.api.deleteDriver(id).subscribe({next:()=>this.load(),error:e=>this.errorMessage=e.error?.message??'Could not delete driver.'});}
}
