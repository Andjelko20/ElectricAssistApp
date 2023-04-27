import { Component, OnInit , ViewChild, ElementRef, Renderer2, AfterViewInit} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ShowDevices } from 'src/app/models/devices.model';
import { DevicesService } from 'src/app/services/devices.service';
import { Categories } from 'src/app/utilities/categories';

@Component({
  selector: 'app-home-devices',
  templateUrl: './home-devices.component.html',
  styleUrls: ['./home-devices.component.css']
})
export class HomeDevicesComponent implements OnInit {

  
  devices:ShowDevices[] = [];
  pageNumber?:number;
  pageSize?:number;
  deviceCategoryId!:number;
  categories=[
    {id:Categories.ELECTRICITY_PRODUCER_ID,name:Categories.ELECTRICITY_PRODUCER_NAME},
    {id:Categories.ELECTRICITY_CONSUMER_ID,name:Categories.ELECTRICITY_CONSUMER_NAME},
    {id:Categories.ELECTRICITY_STOCK_ID,name:Categories.ELECTRICITY_STOCK_NAME}
  ]
  constructor(private router:Router,private deviceService:DevicesService,private renderer: Renderer2,private route:ActivatedRoute) { }
  
  ngOnInit(): void {
    



    this.deviceService.getAllDevices(1,12,2).subscribe(devices => {
     this.devices=devices.data.map((u:any)=>({
      id:u.id,
      userId: u.userId,
      deviceCategory: u.deviceCategory,
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
    }, error => {
      if (error.status === 404) {
        console.log('Devices not found in database');
     }} 
     );
    }
    onSelectedCategory(event:any)
    {
      
      this.deviceCategoryId = event.target.value;
      
      
      this.deviceService.getAllDevices(1,12,this.deviceCategoryId).subscribe(devices => {
        this.devices=devices.data.map((u:any)=>({
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
       }, error => {
        if (error.status === 404) {
          console.log('Devices not found in database');
       }} 
       );
      
      
    }
    

}
 

