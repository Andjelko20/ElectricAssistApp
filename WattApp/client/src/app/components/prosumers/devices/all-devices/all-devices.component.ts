import { Component, ElementRef, HostListener, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ShowDevices } from 'src/app/models/devices.model';
import { AuthService } from 'src/app/services/auth.service';
import { DevicesService } from 'src/app/services/devices.service';
import { SessionService } from 'src/app/services/session.service';
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
  numberOfDevices : number = this.devices.length;
  pageNumber?:number;
  pageSize?:number;
  loader:boolean=false;

  filters : DeviceFilterModel = new DeviceFilterModel(
    0, 
    0, 
    0, 
    -1, 
    -1, 
    -1,
    1, 
    1, 
    0, 
    1, 
    ""
  );

  categories=[
    {id:Categories.ELECTRICITY_PRODUCER_ID,name:Categories.ELECTRICITY_PRODUCER_NAME},
    {id:Categories.ELECTRICITY_CONSUMER_ID,name:Categories.ELECTRICITY_CONSUMER_NAME},
  ]

  showDropdown = false;
  msgShow:boolean=false;
  constructor(
    private router : Router,
    private deviceService : DevicesService,
    private route : ActivatedRoute,
    private elementRef : ElementRef, 
    private authService : AuthService, 
    private filterSession : SessionService) { }

  ngOnInit(): void {
    this.loader=true;
    this.filters = this.filterSession.getSession("filter");
    this.deviceService.getAllDevices(1,this.itemsPerPage,this.filterSession.getSession("filter")).subscribe(devices => {
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
      })as ShowDevices);
      this.numberOfDevices = this.devices.length;
     }, error => {
      if (error.status === 404) {
        this.devices=[]
        console.log('Devices not found in database');
     }} 
     );
    
    }
    
	pageChanged(pageNumber:number){
		this.currentPage=pageNumber;
		this.deviceService.getAllDevices(pageNumber,this.itemsPerPage,this.filterSession.getSession("filter")).subscribe(devices => {
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
			})as ShowDevices);
      this.numberOfDevices = this.devices.length;
		   }, error => {
			if (error.status === 404) {
			  this.devices=[]
			  console.log('Devices not found in database');
		   }} 
		   );
	}


    onSelectedCategory(event:any)
    {this.loader=true;
      this.filters.categoryId = event.target.value;
      this.filterSession.setSession("filter", this.filters);
      this.deviceService.getAllDevices(1,12,this.filterSession.getSession("filter")).subscribe(devices => {
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
        })as ShowDevices);
        this.numberOfDevices = this.devices.length;
       }, error => {
        if (error.status === 404) {
          this.devices=[]
          console.log('Devices not found in database');
       }} 
       );
    }

    onSelectedTurnOn(event:any)
    {this.loader=true;
      this.filters.turnOn = event.target.value;
      this.filterSession.setSession("filter", this.filters);
      this.deviceService.getAllDevices(1,12,this.filterSession.getSession("filter")).subscribe(devices => {
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
        })as ShowDevices);
        this.numberOfDevices = this.devices.length;
       }, error => {
        if (error.status === 404) {
          this.devices=[]
          console.log('Devices not found in database');
       }} 
       );
    }

    onSelectedVisibility(event:any){
      this.loader=true;
      this.filters.visibility = event.target.value;
      this.filterSession.setSession("filter", this.filters);
      this.deviceService.getAllDevices(1,12,this.filterSession.getSession("filter")).subscribe(devices => {
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
        })as ShowDevices);
        this.numberOfDevices = this.devices.length;
       }, error => {
        if (error.status === 404) {
          this.devices=[]
          console.log('Devices not found in database');
       }} 
       );
    }

    onSelectedControlability(event:any){
      this.loader=true;
      this.filters.controlability = event.target.value;
      this.filterSession.setSession("filter", this.filters);
      this.deviceService.getAllDevices(1,12,this.filterSession.getSession("filter")).subscribe(devices => {
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
        })as ShowDevices);
        this.numberOfDevices = this.devices.length;
       }, error => {
        if (error.status === 404) {
          this.devices=[]
          console.log('Devices not found in database');
       }} 
       );
    }

    onEnteredSearchValue(event:any){
      this.loader=true;
      this.filters.searchValue = event.target.value;
      this.filterSession.setSession("filter", this.filters);
      this.deviceService.getAllDevices(1,12,this.filterSession.getSession("filter")).subscribe(devices => {
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
        })as ShowDevices);
        this.numberOfDevices = this.devices.length;
       }, error => {
        if (error.status === 404) {
          this.devices=[]
          console.log('Devices not found in database');
       }} 
       );
    }


    toggleDropdown() {
      this.showDropdown = !this.showDropdown;
    }
    @HostListener('document:click', ['$event'])
    onClick(event: MouseEvent) {
      const clickedElement = event.target as HTMLElement;
      const dropdownElement = this.elementRef.nativeElement;
      //const navbarElement = dropdownElement.querySelector('#dropbtn1') as HTMLElement;
      //const dropdownContent = dropdownElement.querySelector('.dropdown-content') as HTMLElement;
  
      if (!dropdownElement.contains(clickedElement) /*|| (!navbarElement.contains(clickedElement) && !dropdownContent.contains(clickedElement))*/) {
        this.showDropdown = false;
      }
    }

    clearFilters() {
      this.filters = {
        categoryId:0,
        typeId:0,
        brandId:0, 
        turnOn:-1, 
        visibility:-1, 
        controlability:-1, 
        sortCriteria:1, 
        byAscending:1, 
        greaterThan:1, 
        energyInKwhValue:0,
        searchValue:""
      };
      this.filterSession.setSession("filter", this.filters);
      this.pageChanged(1); 
    }

    countActiveFilters(){
      let count = 0;
      if(this.filters.categoryId != 0)
        count++;
      if(this.filters.turnOn != -1)
        count++;
      if(this.filters.visibility != -1)
        count++;
      if(this.filters.controlability != -1)
        count++;
      return count;
    }

}

export class DeviceFilterModel{
  //Filteri
  categoryId : number;
  typeId : number;
  brandId : number;
  turnOn : number;
  visibility : number;
  controlability : number;

  //Sortiranje
  sortCriteria : number;
  byAscending : number;

  //Potrosnja veca/manja od
  energyInKwhValue : number;
  greaterThan : number;

  //Search prema nazivu uredjaja
  searchValue : string;

  constructor(
    categoryId: number,
    typeId: number,
    brandId: number,
    turnOn: number,
    visibility: number,
    contolability: number,
    sortCriteria: number,
    byAscending: number,
    energyInKwhValue: number,
    greaterThan: number,
    searchValue: string
  ) {
    this.categoryId = categoryId;
    this.typeId = typeId;
    this.brandId = brandId;
    this.turnOn = turnOn;
    this.visibility = visibility;
    this.controlability = contolability;
    this.sortCriteria = sortCriteria;
    this.byAscending = byAscending;
    this.energyInKwhValue = energyInKwhValue;
    this.greaterThan = greaterThan;
    this.searchValue = searchValue;
  }


}

enum SortCriteriaValues{
  NAME, 
  ENERGYINKWH
}

