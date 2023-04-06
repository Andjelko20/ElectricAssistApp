import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ShowDevices } from 'src/app/models/devices.model';
import { DevicesService } from 'src/app/services/devices.service';
import { JwtToken } from 'src/app/utilities/jwt-token';

@Component({
  selector: 'app-all-devices',
  templateUrl: './all-devices.component.html',
  styleUrls: ['./all-devices.component.css']
})
export class AllDevicesComponent implements OnInit {

  devices:ShowDevices[] = [];
  constructor(private router:Router,private deviceService:DevicesService,
    private route:ActivatedRoute) { }

  ngOnInit(): void {
    this.deviceService.getAllDevices().subscribe(devices => {
     this.devices=devices
    });

    }
  delete(id:number)
  {
    if(confirm('Are you sere to delete? '+id))
    {
      this.deviceService.delete(id)
      .subscribe({
        next:()=>{
          this.router.navigate(['devices-crud']);
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
