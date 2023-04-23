import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { Prosumers } from 'src/app/models/users.model'
import { ActivatedRoute } from '@angular/router';
import { ShowDevices } from 'src/app/models/devices.model';
import { DevicesService } from 'src/app/services/devices.service';

@Component({
  selector: 'app-dso-one-prosumer-page',
  templateUrl: './dso-one-prosumer-page.component.html',
  styleUrls: ['./dso-one-prosumer-page.component.css']
})
export class DsoOneProsumerPageComponent implements OnInit{
  currentTime!: Date;
  dashboard:boolean = true;
  devices:boolean = false;
  devicesList:ShowDevices[] = []
  deviceCategoryId!: number;
  constructor(private authService:AuthService,private deviceService:DevicesService,private route:ActivatedRoute) {  }

  ngOnInit(): void {
    this.updateTime();
    setInterval(() => {
      this.updateTime();
    }, 1000);
  }

  toggleD()
  {
    this.dashboard = false;
    this.devices = true;
  }
  toggleDa(){
    this.dashboard = true;
    this.devices = false;
  }

  updateTime() {
    this.currentTime = new Date();
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

}
