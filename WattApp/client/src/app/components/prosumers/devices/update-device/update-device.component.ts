import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Devices } from 'src/app/models/devices.model';
import { DevicesService } from 'src/app/services/devices.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-update-device',
  templateUrl: './update-device.component.html',
  styleUrls: ['./update-device.component.css']
})
export class UpdateDeviceComponent implements OnInit{


  updateDevice:Devices={
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
  constructor(private devicesService:DevicesService,private router:Router,private route:ActivatedRoute) { }
  ngOnInit(): void {
    
		
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
