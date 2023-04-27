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

  
  onClickVisibility!: (this: HTMLElement, ev: MouseEvent) => any;
  offClickVisibility!: (this: HTMLElement, ev: MouseEvent) => any;
  onClickControlability!: (this: HTMLElement, ev: MouseEvent) => any;
  offClickControlability!: (this: HTMLElement, ev: MouseEvent) => any;
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
  confirmVisibility: boolean = false;
  confirmControlability:boolean=false;
  devices:ShowDevices[] = [];
  idProsumer?:number;
  idDevice?:number;
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
    this.idDevice=Number(this.route.snapshot.paramMap.get('id'));
  this.devicesService.getDevice( Number(this.route.snapshot.paramMap.get('id')) )
        .subscribe({
          next:(response)=>{
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
    this.confirmControlability=false;
    if(controlabilityOnPopup!=null)
    {
      controlabilityOnPopup.removeEventListener('click', this.onClickControlability);
      this.onClickControlability=()=> {
          this.updateDevice.controlability=true
          this.confirmControlability=true;
          controlabilityOnPopup.removeEventListener('click',this.onClickControlability);
     };
     controlabilityOnPopup.addEventListener('click',this.onClickControlability);
    }
    if(controlabilityOffPopup!=null)
    {
      controlabilityOffPopup.removeEventListener('click', this.offClickControlability);
      this.offClickControlability=()=> {
          this.updateDevice.controlability=false;
          this.confirmControlability=true;
          controlabilityOffPopup.removeEventListener('click',this.offClickControlability);
      };
      controlabilityOffPopup.addEventListener('click',this.offClickControlability);
    }
    
  }
  visibilityOnOff()
  {
    
    const visibilityOnPopup= document.getElementById('visibility-on-popup');
    const visibilityOffPopup= document.getElementById('visibility-off-popup');
    this.confirmVisibility=false;
    if(visibilityOnPopup!=null)
    {
      visibilityOnPopup.removeEventListener('click', this.onClickVisibility);
      this.onClickVisibility=()=> {
        
          this.updateDevice.visibility=true
          this.confirmVisibility=true;
          // console.log( this.updateDevice.visibility);
        
          visibilityOnPopup.removeEventListener('click',this.onClickVisibility);
     };
     visibilityOnPopup.addEventListener('click',this.onClickVisibility);
    
    }
    if(visibilityOffPopup!=null)
    {
      visibilityOffPopup.removeEventListener('click', this.offClickVisibility);
      this.offClickVisibility=()=> {
        
          this.updateDevice.visibility=false;
          this.updateDevice.controlability=false;
          this.confirmVisibility=true;
          // console.log( this.updateDevice.visibility);
        
          visibilityOffPopup.removeEventListener('click',this.offClickVisibility);
     };
     visibilityOffPopup.addEventListener('click',this.offClickVisibility);
    
    }
    
    
  }


}
