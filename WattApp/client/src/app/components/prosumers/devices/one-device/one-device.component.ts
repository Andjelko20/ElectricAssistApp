import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Durations, ShowDevices } from 'src/app/models/devices.model';
import { DevicesService } from 'src/app/services/devices.service';
import { DatePipe } from '@angular/common';
import { Duration } from 'moment';
@Component({
  selector: 'app-one-device',
  templateUrl: './one-device.component.html',
  styleUrls: ['./one-device.component.css']
})
export class OneDeviceComponent implements OnInit{
  
  @ViewChild('modalContent') modalContent!: TemplateRef<any>;
  body: string = ''; 
  btnAction:string='';
  onClick!: (this: HTMLElement, ev: MouseEvent) => any;
  offClick!: (this: HTMLElement, ev: MouseEvent) => any;
  confirm?:boolean=false;
  device!:ShowDevices;
  idDevice?:number;
  buttonOnoff:boolean=false;
  date = new Date();
  formattedDate = this.datePipe.transform(this.date,'yyyy-MM-dd HH:mm:ss');
  duration?:Durations={startTime:"",endTime:"",duration:"0"};
  constructor(private router:Router,private deviceService:DevicesService,
    private route:ActivatedRoute,private modalService: NgbModal, private datePipe: DatePipe ) {
     
     }

    ngOnInit(): void {
      this.idDevice=Number(this.route.snapshot.paramMap.get('id'))
      this.deviceService.getDevice( this.idDevice).subscribe(devices => {
        this.device=devices});

        this.deviceService.durationDateTime(this.idDevice).subscribe(res=>{
          
          if(res.startTime=='/' )
          {
            this.duration={
              startTime:"No date",
              endTime:"No date",
              duration:res.duration,
             }
          }
          else{
            this.duration={
              startTime: res.startTime,
              endTime:res.endTime,
              duration:res.duration,
             }
          }
          
          
        })
        
      }
  delete(id:number)
  {
    this.modalService.open(this.modalContent);
    const deletePopup= document.getElementById('popup');
    this.confirm=false;
   if(deletePopup!=null)
   {this.body="Do you want to delete this device?"
   this.btnAction="Delete";
    deletePopup.addEventListener('click', () => {
      this.deviceService.delete(id)
      .subscribe({
        next:()=>{
          this.router.navigate(['devices']);
          this.confirm=true;
        }
      });
    });
   }
      
    
  }
  
  turnOn(id: number) {
    
    
    this.modalService.open(this.modalContent);

    const turnOn= document.getElementById('popup');
    const date = new Date();
    const formattedDate = this.datePipe.transform(date,'yyyy-MM-dd HH:mm:ss');
    
    
    this.buttonOnoff=false;
   if(turnOn!=null)
   {
    this.body="Do you want to turn on this device?"
    this.btnAction="Turn On";
    turnOn.removeEventListener('click',  this.onClick)
      this.onClick=()=> {
        this.deviceService.turnOn(id,formattedDate).subscribe({
          next:()=>{
            this.deviceService.durationDateTime(id).subscribe(res=>{
          
          
              this.duration={
               startTime: res.startTime,
             endTime:res.endTime,
              duration:res.duration,
               }
              
            })
          
              this.device.turnOn = true;
              this.buttonOnoff=true;
              
            
          }
        });
        turnOn.removeEventListener('click',this.onClick);
      };
      turnOn.addEventListener('click',this.onClick);
    }
   
  }
  turnOff(id: number) {
    
    this.modalService.open(this.modalContent);
   
    const turnOff= document.getElementById('popup');
    
    this.buttonOnoff=false;
    const date = new Date();
    const formattedDate = this.datePipe.transform(date,'yyyy-MM-dd HH:mm:ss');
   
    if(turnOff!=null)
   {
    this.body="Do you want to turn off this device?"
    this.btnAction="Turn Off";
        turnOff.removeEventListener('click',  this.offClick)
        this.offClick=()=> {
          this.deviceService.turnOff(id,formattedDate).subscribe({
            next:()=>{
              this.deviceService.durationDateTime(id).subscribe(res=>{
          
          
                this.duration={
                 startTime: res.startTime,
               endTime:res.endTime,
                duration:res.duration,
                 }
                
              })
                this.device.turnOn = false;
                this.buttonOnoff=true;
                
              
            }
          });
          turnOff.removeEventListener('click',this.offClick);
      };
      turnOff.addEventListener('click',this.offClick);
   }
   
  }

}
