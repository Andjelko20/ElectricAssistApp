import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ShowDevices } from 'src/app/models/devices.model';
import { DevicesService } from 'src/app/services/devices.service';
import { Categories } from 'src/app/utilities/categories';

@Component({
  selector: 'app-all-devices',
  templateUrl: './all-devices.component.html',
  styleUrls: ['./all-devices.component.css']
})
export class AllDevicesComponent implements OnInit {
	currentPage:number=1;
  itemsPerPage:number=12;
  totalItems:number=10;


  devices:ShowDevices[] = [];
  pageNumber?:number;
  pageSize?:number;
  deviceCategoryId!:number;
  categories=[
    {id:Categories.ELECTRICITY_PRODUCER_ID,name:Categories.ELECTRICITY_PRODUCER_NAME},
    {id:Categories.ELECTRICITY_CONSUMER_ID,name:Categories.ELECTRICITY_CONSUMER_NAME},
    {id:Categories.ELECTRICITY_STOCK_ID,name:Categories.ELECTRICITY_STOCK_NAME}
  ]
  constructor(private router:Router,private deviceService:DevicesService,
    private route:ActivatedRoute) { }

  ngOnInit(): void {
    this.deviceCategoryId=0
    this.deviceService.getAllDevices(1,this.itemsPerPage,this.deviceCategoryId).subscribe(devices => {
      this.totalItems=devices.numberOfPages*this.itemsPerPage;
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
        this.devices=[]
        console.log('Devices not found in database');
     }} 
     );
    
    }
    
	pageChanged(pageNumber:number){
		this.currentPage=pageNumber;
		this.deviceService.getAllDevices(pageNumber,this.itemsPerPage,this.deviceCategoryId).subscribe(devices => {
			this.totalItems=devices.numberOfPages*this.itemsPerPage;
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
			  this.devices=[]
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
          this.devices=[]
          console.log('Devices not found in database');
       }} 
       );
      
      
    }
}
