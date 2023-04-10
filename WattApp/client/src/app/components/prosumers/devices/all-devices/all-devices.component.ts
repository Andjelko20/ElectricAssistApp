import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ShowDevices } from 'src/app/models/devices.model';
import { DevicesService } from 'src/app/services/devices.service';

@Component({
  selector: 'app-all-devices',
  templateUrl: './all-devices.component.html',
  styleUrls: ['./all-devices.component.css']
})
export class AllDevicesComponent implements OnInit {

  devices:ShowDevices[] = [];
  pageNumber?:number;
  pageSize?:number;
  constructor(private router:Router,private deviceService:DevicesService,
    private route:ActivatedRoute) { }

  ngOnInit(): void {
    
    this.deviceService.getAllDevices(1,7).subscribe(devices => {
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
    });

    }
  delete(id:number)
  {
    if(confirm('Are you sere to delete? '+id))
    {
      this.deviceService.delete(id)
      .subscribe({
        next:()=>{
          this.router.navigate(['devices']);
          location.reload();
        }
      });
    }
  }
  
  turnOnOff(id: number) {
    console.log(id);
    
      this.deviceService.turnOnOff(id).subscribe({
        next:()=>{
          const userIndex = this.devices.findIndex(device => device.id === id);
          if(this.devices[userIndex].turnOn==false)
          {
            this.devices[userIndex].turnOn = true;
          }   
          else if(this.devices[userIndex].turnOn==true)
          {
            this.devices[userIndex].turnOn = false;
          }
        }
      });
  }
  
  
}
