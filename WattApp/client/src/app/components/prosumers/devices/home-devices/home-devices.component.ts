import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ShowDevices } from 'src/app/models/devices.model';
import { DevicesService } from 'src/app/services/devices.service';

@Component({
  selector: 'app-home-devices',
  templateUrl: './home-devices.component.html',
  styleUrls: ['./home-devices.component.css']
})
export class HomeDevicesComponent implements OnInit {

  devices:ShowDevices[] = [];
  constructor(private router:Router,private deviceService:DevicesService,
    private route:ActivatedRoute) { }

  ngOnInit(): void {
    this.deviceService.getAllDevices().subscribe(devices => {
     this.devices=devices
    });

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
