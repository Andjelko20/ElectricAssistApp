import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ShowDevices } from 'src/app/models/devices.model';
import { AuthService } from 'src/app/services/auth.service';
import { DevicesService } from 'src/app/services/devices.service';
import { Categories } from 'src/app/utilities/categories';

@Component({
  selector: 'app-all-devices-dso',
  templateUrl: './all-devices-dso.component.html',
  styleUrls: ['./all-devices-dso.component.css']
})
export class AllDevicesDsoComponent implements OnInit{
  
  devicesList:ShowDevices[] = []
  deviceCategoryId!: number;
  idDevice!: number;
  constructor(private authService:AuthService,private deviceService:DevicesService,private route:ActivatedRoute) {}
  categories=[

     {id:Categories.ELECTRICITY_PRODUCER_ID,name:Categories.ELECTRICITY_PRODUCER_NAME},
    
     {id:Categories.ELECTRICITY_CONSUMER_ID,name:Categories.ELECTRICITY_CONSUMER_NAME},
    
     {id:Categories.ELECTRICITY_STOCK_ID,name:Categories.ELECTRICITY_STOCK_NAME}
    
     ]

  ngOnInit(): void {
    this.deviceCategoryId = 2;
    this.deviceService.getDeviceProsumer(Number(this.route.snapshot.paramMap.get('id')),1,12,this.deviceCategoryId).subscribe(devices => {
      this.devicesList=devices.data.map((u:any)=>({
        id:u.id,
        userId: u.userId,
        deviceCategory:u.deviceCategory,
        deviceType: u.deviceType ,
        deviceBrand: u.deviceBrand ,
        deviceModel: u.deviceModel ,
        name: u.name ,
        energyInKwh: u.energyInKwh,
        standByKwh: u.standByKwh,
        visibility: u.visibility,
        controlability: u.controlability,
        turnOn: u.turnOn,
    
    })as ShowDevices)
    
    }, (error: { status: number; }) => {
    
    if (error.status === 404) {
    
      this.devicesList=[]
    
      console.log('Devices not found in database');
    
    }}
    
    );
    
  }

  onSelectedCategory(event:any){

    this.deviceCategoryId = event.target.value;
    this.deviceService.getDeviceProsumer(Number(this.route.snapshot.paramMap.get('id')),1,12,this.deviceCategoryId).subscribe(devices => {
      this.devicesList=devices.data.map((u:any)=>({
        id:u.id,
        userId: u.userId,
        deviceCategory:u.deviceCategory,
        deviceType: u.deviceType ,
        deviceBrand: u.deviceBrand ,
        deviceModel: u.deviceModel ,
        name: u.name ,
        energyInKwh: u.energyInKwh,
        standByKwh: u.standByKwh,
        visibility: u.visibility,
        controlability: u.controlability,
        turnOn: u.turnOn,
    
    })as ShowDevices)
    
    }, (error: { status: number; }) => {
    
    if (error.status === 404) {
    
      this.devicesList=[]
    
      console.log('Devices not found in database');
    
    }}
    
    );
    }
    turnOnOff(id: number) {

       console.log(id);
      this.idDevice=Number(this.route.snapshot.paramMap.get('id'))
      this.deviceService.turnOnOff(id).subscribe({
      next:()=>{
        const userIndex = this.devicesList.findIndex(user => user.id === id);
        
      if(this.devicesList[userIndex].turnOn == false)
      {
      this.devicesList[userIndex].turnOn = true;
      } 
      else if(this.devicesList[userIndex].turnOn == true)
      {
        this.devicesList[userIndex].turnOn = false;
      }
      
      }
      
      });
      
       }

}
