import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ShowDevices } from 'src/app/models/devices.model';
import { AuthService } from 'src/app/services/auth.service';
import { DevicesService } from 'src/app/services/devices.service';
import { Categories } from 'src/app/utilities/categories';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
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
  datePipe: any;
  currentPage:number=1;
  itemsPerPage:number=12;
  totalItems:number=10;
  onClick!: (this: HTMLElement, ev: MouseEvent) => any;
  offClick!: (this: HTMLElement, ev: MouseEvent) => any;
  
  devicesList:ShowDevices[] = [];
  deviceCategoryId!: number;
  idDevice!: number;
  constructor(private authService:AuthService,private deviceService:DevicesService,private route:ActivatedRoute,private modalService: NgbModal) {}
  categories=[

     {id:Categories.ELECTRICITY_PRODUCER_ID,name:Categories.ELECTRICITY_PRODUCER_NAME},
    
     {id:Categories.ELECTRICITY_CONSUMER_ID,name:Categories.ELECTRICITY_CONSUMER_NAME},
    
     {id:Categories.ELECTRICITY_STOCK_ID,name:Categories.ELECTRICITY_STOCK_NAME}
    
     ]

  ngOnInit(): void {
   
    
    this.deviceCategoryId = 2;
    this.deviceService.getDeviceProsumer(Number(this.route.snapshot.paramMap.get('id')),1,this.itemsPerPage,this.deviceCategoryId).subscribe(devices => {
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
    
    }, (error: { status: number; }) => {
    
    if (error.status === 404) {
    
      this.devicesList=[]
    
      //console.log('Devices not found in database');
    
    }}
    
    );
    
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
	  
	  }, (error: { status: number; }) => {
	  
	  if (error.status === 404) {
	  
		this.devicesList=[]
	  
		//console.log('Devices not found in database');
	  
	  }}
	  
	  );
  }

  onSelectedCategory(event:any){

    this.deviceCategoryId = event.target.value;
    this.deviceService.getDeviceProsumer(Number(this.route.snapshot.paramMap.get('id')),1,this.itemsPerPage,this.deviceCategoryId).subscribe(devices => {
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
    
    }, (error: { status: number; }) => {
    
    if (error.status === 404) {
    
      this.devicesList=[]
    
      //console.log('Devices not found in database');
    
    }}
    
    );
    }
    
       turnOn(id: number) {
        this.modalService.open(this.modalContent);
        this.confirmTurnOnOff=false;
        const turnOn= document.getElementById('popup');
       
        //console.log(turnOn);
       if(turnOn!=null)
       {
        const date = new Date();
        const formattedDate = this.datePipe.transform(date,'yyyy-MM-dd hh:mm:ss');
        this.body = 'Do you want to turn on this device?';
        this.btnAction="Turn On";
            turnOn.removeEventListener('click', this.onClick);
            this.onClick=() => {
              this.idDevice=Number(this.route.snapshot.paramMap.get('id'))
              this.deviceService.turnOn(id,formattedDate).subscribe({
              next:()=>{
                const userIndex = this.devicesList.findIndex(user => user.id === id);
                
                  this.devicesList[userIndex].turnOn = true;
                  this.confirmTurnOnOff=true;
              }
              
              });
              turnOn.removeEventListener('click', this.onClick);
          };
          turnOn.addEventListener('click', this.onClick);
       }
      
       
      }
      turnOff(id: number){
        const date = new Date();
        const formattedDate = this.datePipe.transform(date,'yyyy-MM-dd hh:mm:ss');
        this.modalService.open(this.modalContent);
        this.confirmTurnOnOff=false;
        
        const turnOff= document.getElementById('popup');
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
