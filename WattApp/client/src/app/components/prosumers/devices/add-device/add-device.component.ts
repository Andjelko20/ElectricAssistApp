
import { Component, InjectionToken, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Categories } from '../../../../utilities/categories'
import { DevicesService } from 'src/app/services/devices.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { environment } from 'src/environments/environment';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

let typeID:number=0;
@Component({
  selector: 'app-add-device',
  templateUrl: './add-device.component.html',
  styleUrls: ['./add-device.component.css']
})
export class AddDeviceComponent implements OnInit{

  myForm: FormGroup;
  addDeviceRequest={
    userId: 0,
    deviceCategoryId: 0 ,
    deviceTypeId: 0 ,
    deviceBrandId: 0 ,
    deviceModelId: 0 ,
    name: '' ,
    energyInKwh: 0,
    standByKwh: 0,
    visibility: false,
    controlability: false,
    turnOn: false
  }
  types:Array<any>=[];
  brands:Array<any>=[];
  models:Array<any>=[];
  idProsumer?:number;

  categories=[
    {id:Categories.ELECTRICITY_PRODUCER_ID,name:Categories.ELECTRICITY_PRODUCER_NAME},
    {id:Categories.ELECTRICITY_CONSUMER_ID,name:Categories.ELECTRICITY_CONSUMER_NAME},
    {id:Categories.ELECTRICITY_STOCK_ID,name:Categories.ELECTRICITY_STOCK_NAME}
  ]

  constructor(private devicesService:DevicesService,private router:Router,private formBuilder: FormBuilder) {
    this.addDeviceRequest.deviceCategoryId=this.categories[0]?.id
    this.myForm = this.formBuilder.group({
      nameform1: ['', Validators.required],
       nameform2: ['', Validators.required],
       nameform3: ['', Validators.required]
      
    });
   }
 
  ngOnInit(): void {
      
      
    let token=new JwtToken();
    this.idProsumer=token.data.id as number;
   
    this.addDeviceRequest.userId=this.idProsumer;
    
  }
  
  addDevices()
  {
    
    
    this.devicesService.addDevices(this.addDeviceRequest)
    .subscribe({
      next:()=>{
         this.router.navigate(['/devices']);
      
      }
    });
  }
  onSelectedCategory(event:any)
  {
    
    
    this.addDeviceRequest.deviceCategoryId = event.target.value;
    console.log(this.addDeviceRequest.deviceCategoryId );
    fetch(environment.serverUrl+"/types?categoryId="+this.addDeviceRequest.deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
    .then(res=>res.json())
    .then(res=>{
      this.types=res;
     
    });
  }
  onSelectType(event:any)
  {
    this.addDeviceRequest.deviceTypeId = event.target.value;
    typeID=this.addDeviceRequest.deviceTypeId;
    console.log(this.addDeviceRequest.deviceTypeId );
    
    
    fetch(environment.serverUrl+"/brands?typeId="+this.addDeviceRequest.deviceTypeId,{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
    .then(res=>res.json())
    .then(res=>{
      this.brands=res;
      
   
    });

  }
  
  onSelectBrand(event:any)
  {
    
    this.addDeviceRequest.deviceBrandId = event.target.value;
    console.log( this.addDeviceRequest.deviceBrandId);
    fetch(environment.serverUrl+"/models?typeId="+typeID+"&brandId="+this.addDeviceRequest.deviceBrandId,{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
    .then(res=>res.json())
    .then(res=>{
      this.models=res;
      console.log(res);
      
    });
  }
  
  onSelectModel(event:any)
  {
    this.addDeviceRequest.deviceModelId = event.target.value;
    console.log( this.addDeviceRequest.deviceModelId);
  }
}
