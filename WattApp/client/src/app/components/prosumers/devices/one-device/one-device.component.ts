import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ShowDevices } from 'src/app/models/devices.model';
import { DevicesService } from 'src/app/services/devices.service';

@Component({
  selector: 'app-one-device',
  templateUrl: './one-device.component.html',
  styleUrls: ['./one-device.component.css']
})
export class OneDeviceComponent implements OnInit{
  
  onClick!: (this: HTMLElement, ev: MouseEvent) => any;
  offClick!: (this: HTMLElement, ev: MouseEvent) => any;
  device!:ShowDevices;
  idDevice?:number;
  buttonOnoff:boolean=false;
  constructor(private router:Router,private deviceService:DevicesService,
    private route:ActivatedRoute) {
     
     }

    ngOnInit(): void {
      this.idDevice=Number(this.route.snapshot.paramMap.get('id'))
      this.deviceService.getDevice( this.idDevice).subscribe(devices => {
        this.device=devices})
  
      }
  delete(id:number)
  {
    const deletePopup= document.getElementById('delete-popup');
    
   if(deletePopup!=null)
   {
    deletePopup.addEventListener('click', () => {
      this.deviceService.delete(id)
      .subscribe({
        next:()=>{
          this.router.navigate(['devices']);
        }
      });
    });
   }
      
    
  }
  
  turnOnOff(id: number) {
    //console.log(id);
    
    const turnOn= document.getElementById('turn-on-popup');
    const turnOff= document.getElementById('turn-off-popup');
    this.buttonOnoff=false;
   if(turnOn!=null)
   {
    turnOn.removeEventListener('click',  this.onClick)
      this.onClick=()=> {
        this.deviceService.turnOnOff(id).subscribe({
          next:()=>{
          
            
              this.device.turnOn = true;
              this.buttonOnoff=true;
            
          }
        });
        turnOn.removeEventListener('click',this.onClick);
      };
      turnOn.addEventListener('click',this.onClick);
    }
   if(turnOff!=null)
   {
        
        turnOff.removeEventListener('click',  this.offClick)
        this.offClick=()=> {
          this.deviceService.turnOnOff(id).subscribe({
            next:()=>{
            
              
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
