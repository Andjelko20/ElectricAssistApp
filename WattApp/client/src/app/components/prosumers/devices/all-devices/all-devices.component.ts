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
  loader:boolean=false;

  categories=[
    {id:Categories.ELECTRICITY_PRODUCER_ID,name:Categories.ELECTRICITY_PRODUCER_NAME},
    {id:Categories.ELECTRICITY_CONSUMER_ID,name:Categories.ELECTRICITY_CONSUMER_NAME},
    
  ]
  constructor(private router:Router,private deviceService:DevicesService,
    private route:ActivatedRoute) { }

  ngOnInit(): void {
    this.loader=true;
    this.deviceCategoryId=0
    this.deviceService.getAllDevices(1,this.itemsPerPage,this.deviceCategoryId).subscribe(devices => {
      this.totalItems=devices.numberOfPages*this.itemsPerPage;
      this.loader=false;
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
    {this.loader=true;
      this.deviceCategoryId = event.target.value;
      this.deviceService.getAllDevices(1,12,this.deviceCategoryId).subscribe(devices => {
        this.loader=false;
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

class DeviceFilterModel{
  //Filteri
  categoryId : number;
  typeId : number;
  brandId : number;
  turnOn : boolean;
  visibility : boolean;
  contolability : boolean;

  //Sortiranje
  sortCriteria : SortCriteriaValues;
  byAscending : boolean;

  //Potrosnja veca/manja od
  energyInKwhValue : number;
  greaterThan : boolean;

  //Search prema nazivu uredjaja
  searchValue : string;

  constructor(
    categoryId: number,
    typeId: number,
    brandId: number,
    turnOn: boolean,
    visibility: boolean,
    contolability: boolean,
    sortCriteria: SortCriteriaValues,
    byAscending: boolean,
    energyInKwhValue: number,
    greaterThan: boolean,
    searchValue: string
  ) {
    this.categoryId = categoryId;
    this.typeId = typeId;
    this.brandId = brandId;
    this.turnOn = turnOn;
    this.visibility = visibility;
    this.contolability = contolability;
    this.sortCriteria = sortCriteria;
    this.byAscending = byAscending;
    this.energyInKwhValue = energyInKwhValue;
    this.greaterThan = greaterThan;
    this.searchValue = searchValue;
  }


}


enum SortCriteriaValues{
  NAME, 
  ENERGYINKWH, 
}
