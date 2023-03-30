import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Devices } from 'src/app/models/devices.model';
import { DevicesService } from 'src/app/services/devices.service';

@Component({
  selector: 'app-all-devices',
  templateUrl: './all-devices.component.html',
  styleUrls: ['./all-devices.component.css']
})
export class AllDevicesComponent implements OnInit {

  devices:Devices[] = [];
  constructor(private router:Router,private deviceService:DevicesService,
    private route:ActivatedRoute) { }

  ngOnInit(): void {
    this.deviceService.getAllDevices().subscribe(devices => {
      console.log(devices);
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
//   turnOn(id: number) {
//     this.deviceService.turnOn(id).subscribe(() => {
//       const deviceIndex = this.devices.findIndex(device => device.id === id);
//       this.devices[deviceIndex].turnOn = true;
//     });
// }
// turnOff(id: number) {
//   this.deviceService.turnOn(id).subscribe(() => {
//     const deviceIndex = this.devices.findIndex(device => device.id === id);
//     this.devices[deviceIndex].turnOn = false;
//   });
// }

}
