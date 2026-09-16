import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Api, Vehicle, VehicleStatus } from '../../services/api';
@Component({ selector:'app-vehicles', standalone:true, imports:[FormsModule], templateUrl:'./vehicles.html', styleUrl:'./vehicles.css' })
export class Vehicles implements OnInit {
 private readonly api=inject(Api); vehicles:Vehicle[]=[]; search=''; showForm=false; editing:Vehicle|null=null; errorMessage=''; statuses=[1,2,3,4];
 form={plateNumber:'',model:'',type:'',year:new Date().getFullYear(),capacityKg:0,status:VehicleStatus.Available};
 ngOnInit(){this.load();} load(){this.api.getVehicles().subscribe({next:d=>this.vehicles=d,error:e=>this.errorMessage=e.error?.message??'Could not load vehicles.'});}
 filtered(){const q=this.search.toLowerCase();return this.vehicles.filter(v=>`${v.plateNumber} ${v.model} ${v.type}`.toLowerCase().includes(q));} statusName(s:number){return VehicleStatus[s]??String(s);}
 openNew(){this.editing=null;this.form={plateNumber:'',model:'',type:'',year:new Date().getFullYear(),capacityKg:0,status:VehicleStatus.Available};this.showForm=true;} edit(v:Vehicle){this.editing=v;this.form={plateNumber:v.plateNumber,model:v.model,type:v.type,year:v.year,capacityKg:v.capacityKg,status:v.status};this.showForm=true;}
 save(){const req=this.editing?this.api.updateVehicle({id:this.editing.id,...this.form}):this.api.createVehicle({plateNumber:this.form.plateNumber,model:this.form.model,type:this.form.type,year:this.form.year,capacityKg:this.form.capacityKg});req.subscribe({next:()=>{this.showForm=false;this.load();},error:e=>this.errorMessage=e.error?.message??'Could not save vehicle.'});}
 remove(id:number){if(confirm('Delete this vehicle?'))this.api.deleteVehicle(id).subscribe({next:()=>this.load(),error:e=>this.errorMessage=e.error?.message??'Could not delete vehicle.'});}
}
