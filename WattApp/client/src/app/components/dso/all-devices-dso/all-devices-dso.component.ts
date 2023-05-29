import { Component, HostListener, OnInit, TemplateRef, ViewChild ,ElementRef} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ShowDevices } from 'src/app/models/devices.model';
import { AuthService } from 'src/app/services/auth.service';
import { DevicesService } from 'src/app/services/devices.service';
import { Categories } from 'src/app/utilities/categories';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DatePipe } from '@angular/common';
import { DeviceFilterModel } from '../../prosumers/devices/all-devices/all-devices.component';
import { environment } from 'src/environments/environment';
@Component({
  selector: 'app-all-devices-dso',
  templateUrl: './all-devices-dso.component.html',
  styleUrls: ['./all-devices-dso.component.css']
})
export class AllDevicesDsoComponent implements OnInit{
  @ViewChild('modalContent') modalContent!: TemplateRef<any>;
  @ViewChild('modalContent1') modalContent1!: TemplateRef<any>;
 
  body: string = ''; 
  btnAction:string='';
  confirmTurnOnOff?:boolean=false;
  currentPage:number=1;
  itemsPerPage:number=12;
  totalItems:number=10;
  oNClick!: (this: HTMLElement, ev: MouseEvent) => any;
  offClick!: (this: HTMLElement, ev: MouseEvent) => any;
  
  devicesList:ShowDevices[] = [];
  numberOfDevices : number = 0;
  deviceCategoryId!: number;
  idDevice!: number;
  deviceCategory?:boolean=false;
  showDropdown:boolean=false;

  filters : DeviceFilterModel = new DeviceFilterModel(
      0, 
      0, 
      0, 
      -1, 
      -1, 
      -1,
      0, 
      1, 
      0, 
      1, 
      ""
  )
  

  constructor(private elementRef: ElementRef,private authService:AuthService,private deviceService:DevicesService,private route:ActivatedRoute,private modalService: NgbModal,private datePipe: DatePipe) {}
  
  categories=[
     {id:Categories.ELECTRICITY_PRODUCER_ID,name:Categories.ELECTRICITY_PRODUCER_NAME},
     {id:Categories.ELECTRICITY_CONSUMER_ID,name:Categories.ELECTRICITY_CONSUMER_NAME},
  ]

  types : Array<any> = [];
  brands : Array<any> = [];

  ngOnInit(): void {
    this.deviceService.getDeviceProsumer(Number(this.route.snapshot.paramMap.get('id')),1,this.itemsPerPage,this.filters).subscribe(devices => {
    	this.totalItems=devices.numberOfPages*this.itemsPerPage;
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
    console.log(this.devicesList);
    this.setNumberOfDevices();
    }, (error: { status: number; }) => {
    
    if (error.status === 404) {
    
      this.devicesList=[]
    
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
    const navbarElement = dropdownElement.querySelector('#dropbtn1') as HTMLElement;
    const dropdownContent = dropdownElement.querySelector('.dropdown-content') as HTMLElement;

    if (!dropdownElement.contains(clickedElement) || (!navbarElement.contains(clickedElement) && !dropdownContent.contains(clickedElement))) {
      this.showDropdown = false;
    }
  }
  validateFormInput(input: any) {
    const value = input.value.trim();
    const regex = /^\d+(\.\d+)?$/;
  
    if (!regex.test(value) && value != "") {
      input.style.border = '2px solid red';
    } else {
      input.style.borderColor = '';
    }
  }

  pageChanged(pageNumber:number){
	this.currentPage=pageNumber;
	this.deviceService.getDeviceProsumer(Number(this.route.snapshot.paramMap.get('id')),pageNumber,this.itemsPerPage,this.deviceCategoryId).subscribe(devices => {
		this.totalItems=devices.numberOfPages*this.itemsPerPage;
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
	  this.setNumberOfDevices();
	  }, (error: { status: number; }) => {
	  
	  if (error.status === 404) {
	  
		this.devicesList=[]
	  
		//console.log('Devices not found in database');
	  
	  }}
	  
	  );
  }

  onSelectedCategory(event:any){
    this.filters.categoryId = event.target.value;
    this.filters.brandId = 0;
    this.filters.typeId = 0;

    fetch(environment.serverUrl+"/types?categoryId="+this.filters.categoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
    .then(res=>res.json())
    .then(res=>{
      this.types=res;
    });

    this.deviceService.getDeviceProsumer(Number(this.route.snapshot.paramMap.get('id')),1,this.itemsPerPage,this.filters).subscribe(devices => {
      this.totalItems=devices.numberOfPages*this.itemsPerPage;
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
    this.setNumberOfDevices();
    }, (error: { status: number; }) => {
    
    if (error.status === 404) {
    
      this.devicesList=[]
    
    }}
    
    );
    }

    onSelectedType(event : any){
      this.filters.typeId = event.target.value;
      this.filters.brandId = 0;

      fetch(environment.serverUrl+"/brands?typeId="+this.filters.typeId,{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
        .then(res=>res.json())
        .then(res=>{
        this.brands=res;
      });

      this.deviceService.getDeviceProsumer(Number(this.route.snapshot.paramMap.get('id')),1,this.itemsPerPage,this.filters).subscribe(devices => {
        this.totalItems=devices.numberOfPages*this.itemsPerPage;
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
      this.setNumberOfDevices();
      }, (error: { status: number; }) => {
      if (error.status === 404) {
        this.devicesList=[]
      }}
      );
    }

    onSelectedBrand(event : any){
      this.filters.brandId = event.target.value;

      this.deviceService.getDeviceProsumer(Number(this.route.snapshot.paramMap.get('id')),1,this.itemsPerPage,this.filters).subscribe(devices => {
        this.totalItems=devices.numberOfPages*this.itemsPerPage;
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
      this.setNumberOfDevices();
      }, (error: { status: number; }) => {
      if (error.status === 404) {
        this.devicesList=[]
      }}
      );
    }

    onSelectedTurnOn(event : any){
      this.filters.turnOn = event.target.value;

      this.deviceService.getDeviceProsumer(Number(this.route.snapshot.paramMap.get('id')),1,this.itemsPerPage,this.filters).subscribe(devices => {
        this.totalItems=devices.numberOfPages*this.itemsPerPage;
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
      this.setNumberOfDevices();
      }, (error: { status: number; }) => {
      if (error.status === 404) {
        this.devicesList=[]
      }}
      );
    }

    onSelectedControlability(event : any){
      this.filters.controlability = event.target.value;

      this.deviceService.getDeviceProsumer(Number(this.route.snapshot.paramMap.get('id')),1,this.itemsPerPage,this.filters).subscribe(devices => {
        this.totalItems=devices.numberOfPages*this.itemsPerPage;
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
      this.setNumberOfDevices();
      }, (error: { status: number; }) => {
      if (error.status === 404) {
        this.devicesList=[]
      }}
      );
    }

    onSelectedGreaterThan(event : any){
      this.filters.greaterThan = event.target.value;

      this.deviceService.getDeviceProsumer(Number(this.route.snapshot.paramMap.get('id')),1,this.itemsPerPage,this.filters).subscribe(devices => {
        this.totalItems=devices.numberOfPages*this.itemsPerPage;
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
      this.setNumberOfDevices();
      }, (error: { status: number; }) => {
      if (error.status === 404) {
        this.devicesList=[]
      }}
      );
    }

    onSelectedEnergyInKwhValue(event : any){
      this.filters.energyInKwhValue = event.target.value;

      this.deviceService.getDeviceProsumer(Number(this.route.snapshot.paramMap.get('id')),1,this.itemsPerPage,this.filters).subscribe(devices => {
        this.totalItems=devices.numberOfPages*this.itemsPerPage;
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
      this.setNumberOfDevices();
      }, (error: { status: number; }) => {
      if (error.status === 404) {
        this.devicesList=[]
      }}
      );
    }

    onSelectedSearchValue(event : any){
      this.filters.searchValue = event.target.value;

      this.deviceService.getDeviceProsumer(Number(this.route.snapshot.paramMap.get('id')),1,this.itemsPerPage,this.filters).subscribe(devices => {
        this.totalItems=devices.numberOfPages*this.itemsPerPage;
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
      this.setNumberOfDevices();
      }, (error: { status: number; }) => {
      if (error.status === 404) {
        this.devicesList=[]
      }}
      );
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
      this.pageChanged(1); 
    }

    reset(){
      this.filters.greaterThan = 1;
      this.filters.energyInKwhValue = 0;
    }    

    setNumberOfDevices(){
      this.numberOfDevices = this.devicesList.length;
    }

       turnOn(id: number) {
        this.modalService.open(this.modalContent);
        this.confirmTurnOnOff=false;
        const turnOn= document.getElementById('popup');
       
        const date = new Date();
    const formattedDate = this.datePipe.transform(date,'yyyy-MM-dd HH:mm:ss');
   
       if(turnOn!=null)
       {
         this.body = 'Do you want to turn on this device?';
        this.btnAction="Turn On";
            turnOn.removeEventListener('click', this.oNClick);
            this.oNClick=() => {
              this.idDevice=Number(this.route.snapshot.paramMap.get('id'))
              this.deviceService.turnOn(id,formattedDate).subscribe({
              next:()=>{
                const userIndex = this.devicesList.findIndex(user => user.id === id);
                
                  this.devicesList[userIndex].turnOn = true;
                  this.confirmTurnOnOff=true;
              }
              
              });
              turnOn.removeEventListener('click', this.oNClick);
          };
          turnOn.addEventListener('click', this.oNClick);
       }
      
       
      }
      turnOff(id: number){
        this.modalService.open(this.modalContent);
        this.confirmTurnOnOff=false;
        
        const turnOff= document.getElementById('popup');
        const date = new Date();
    const formattedDate = this.datePipe.transform(date,'yyyy-MM-dd HH:mm:ss');
   
        if(turnOff!=null)
       {
          this.body = 'Do you want to turn off this device?';
          this.btnAction="Turn Off";
          turnOff.removeEventListener('click', this.offClick);
              this.offClick=() => {
                this.idDevice=Number(this.route.snapshot.paramMap.get('id'))
                this.deviceService.turnOff(id,formattedDate).subscribe({
                next:()=>{
                  const userIndex = this.devicesList.findIndex(user => user.id === id);
                  
                    this.devicesList[userIndex].turnOn = false;
                    this.confirmTurnOnOff=true;
                }
                
                });
                turnOff.removeEventListener('click', this.offClick);
            };
            turnOff.addEventListener('click', this.offClick);
        }
       
      }
      noControl()
      {
        this.modalService.open(this.modalContent1);
        this.body = 'You dont have permission to turn on or off this device!';
        

      }

}
