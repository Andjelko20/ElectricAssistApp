import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Devices, ShowDevices } from 'src/app/models/devices.model';
import { DevicesService } from 'src/app/services/devices.service';

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
          this.router.navigate(['/admindso']);
          location.reload();
        }
      });
    }
  }
  
  // turnOnOff(id: number) {
  //   console.log(id);
    
  //     this.deviceService.turnOn(id).subscribe({
  //       next:()=>{
  //         this.router.navigate(['/devices']);
          
  //       }
  //     });
  // }
  turnOn(id: number) {
    console.log(id);
    
      this.deviceService.turnOn(id).subscribe({
        next:()=>{
          this.router.navigate(['/devices']);
          location.reload()
        }
      });
  }
  turnOff(id: number) {
    
    this.deviceService.turnOff(id).subscribe({
      next:()=>{
        this.router.navigate(['/devices']);
        location.reload()
      }
    });
}
  

}
