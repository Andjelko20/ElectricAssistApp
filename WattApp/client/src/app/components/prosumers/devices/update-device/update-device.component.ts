import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import {  ShowDevices, updateDevices } from 'src/app/models/devices.model';
import { DevicesService } from 'src/app/services/devices.service';
import { JwtToken } from 'src/app/utilities/jwt-token';


@Component({
  selector: 'app-update-device',
  templateUrl: './update-device.component.html',
  styleUrls: ['./update-device.component.css']
})
export class UpdateDeviceComponent implements OnInit{

  myForm: FormGroup;
  updateDevice:updateDevices={
    id: 0,
    userId: 0,
    name: '',
    energyInKwh: 0,
    standByKwh: 0,
    visibility: false,
    controlability: false,
    turnOn: false,
    
  }
  devices:ShowDevices[] = [];
  idProsumer?:number
  constructor(private devicesService:DevicesService,private router:Router,private route:ActivatedRoute,private formBuilder: FormBuilder) {
    this.myForm = this.formBuilder.group({
      nameform1: ['', Validators.required],
       nameform2: ['', Validators.required],
       nameform3: ['', Validators.required]
      
    });
   }
  ngOnInit(): void {
    
    let token=new JwtToken();
    this.idProsumer=token.data.id as number;
    console.log(this.idProsumer);
    
    

  this.devicesService.getDevice( Number(this.route.snapshot.paramMap.get('id')) )
        .subscribe({
          next:(response)=>{
            console.log(this.idProsumer);
            this.updateDevice={
             
              
              id:Number(this.route.snapshot.paramMap.get('id')),
              userId: response.userId,
              name: response.name,
              energyInKwh: response.energyInKwh,
              standByKwh: response.standByKwh,
              visibility: response.visibility,
              controlability: response.controlability,
              turnOn: response.turnOn,
              };
            },
			error:(response)=>{
				this.router.navigate(["/devices"]);
			}
          });

   
  }
  
  upDate()
  {
    
    this.devicesService.upDateDevice(this.updateDevice)
    .subscribe({
      next:()=>{
        this.router.navigate(['device/'+Number(this.route.snapshot.paramMap.get('id'))]);
      }
    });
  }

  controlabilityOnOff(){
    const controlabilityOnPopup= document.getElementById('controlability-on-popup');
    const controlabilityOffPopup= document.getElementById('controlability-off-popup');
    
    if(controlabilityOnPopup!=null)
    {
      controlabilityOnPopup.addEventListener('click', () => {
        if( this.updateDevice.controlability==false && this.updateDevice.visibility==true)
        {
          this.updateDevice.controlability=true
          console.log( this.updateDevice.controlability);
        }
        
     });
    }
    if(controlabilityOffPopup!=null)
    {
      controlabilityOffPopup.addEventListener('click', () => {
        if( this.updateDevice.controlability==true )
        {
          this.updateDevice.controlability=false
          console.log( this.updateDevice.controlability);
        }
     });
    }
    
  }
  visibilityOnOff()
  {
    
    const visibilityOnPopup= document.getElementById('visibility-on-popup');
    const visibilityOffPopup= document.getElementById('visibility-off-popup');
    console.log( visibilityOnPopup);
    console.log( visibilityOffPopup);
    if(visibilityOnPopup!=null)
    {
      visibilityOnPopup.addEventListener('click', () => {
        if( this.updateDevice.visibility==false)
        {
          this.updateDevice.visibility=true
          console.log( this.updateDevice.visibility);
        }
        
     });
    }
    if(visibilityOffPopup!=null)
    {
      visibilityOffPopup.addEventListener('click', () => {
        if( this.updateDevice.visibility==true)
        {
          this.updateDevice.visibility=false
          console.log( this.updateDevice.visibility);
        }
     });
    }
    
    
  }



}
