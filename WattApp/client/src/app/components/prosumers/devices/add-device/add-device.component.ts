
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UpdateDevices } from 'src/app/models/devices.model';

import { DevicesService } from 'src/app/services/devices.service';


@Component({
  selector: 'app-add-device',
  templateUrl: './add-device.component.html',
  styleUrls: ['./add-device.component.css']
})
export class AddDeviceComponent {

  
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
  
  roles:Array<any>=[];
  
  constructor(private devicesService:DevicesService,private router:Router) { }

  ngOnInit(): void {
	
  }
  
  addDevices()
  {
    console.log(this.addDeviceRequest);
    
    this.devicesService.addDevices(this.addDeviceRequest)
    .subscribe({
      next:()=>{
         this.router.navigate(['/devices']);
      
      }
    });
  }
}
