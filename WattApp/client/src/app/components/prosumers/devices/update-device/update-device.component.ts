import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {  updateDevices } from 'src/app/models/devices.model';
import { DevicesService } from 'src/app/services/devices.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-update-device',
  templateUrl: './update-device.component.html',
  styleUrls: ['./update-device.component.css']
})
export class UpdateDeviceComponent implements OnInit{


  updateDevice:updateDevices={
    id: 0,
    userId: 0,
    deviceCategoryId: 0,
    deviceTypeId: 0,
    deviceBrandId: 0,
    deviceModelId: 0,
    name: '',
    energyInKwh: 0,
    standByKwh: 0,
    visibility: false,
    controlability: false,
    turnOn: false,
    
  }

  idProsumer?:number
  constructor(private devicesService:DevicesService,private router:Router,private route:ActivatedRoute) { }
  ngOnInit(): void {
    
    let token=new JwtToken();
    this.idProsumer=token.data.id as number;

    

  this.devicesService.getDevice( Number(this.route.snapshot.paramMap.get('id')) )
        .subscribe({
          next:(response)=>{
            this.updateDevice={
              id:Number(this.route.snapshot.paramMap.get('id')),
              userId: response.userId,
              deviceCategoryId: response.deviceCategoryId,
              deviceTypeId: response.deviceTypeId,
              deviceBrandId: response.deviceBrandId,
              deviceModelId: response.deviceModelId,
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
        this.router.navigate(['/devices']);
      }
    });
  }





}
