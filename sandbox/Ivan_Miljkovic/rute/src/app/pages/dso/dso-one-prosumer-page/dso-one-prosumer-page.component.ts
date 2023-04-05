import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dso-one-prosumer-page',
  templateUrl: './dso-one-prosumer-page.component.html',
  styleUrls: ['./dso-one-prosumer-page.component.css']
})
export class DsoOneProsumerPageComponent implements OnInit{
  currentTime!: Date;
  dashboard:boolean = true;
  devices:boolean = false;

  constructor() { }

  ngOnInit(): void {
    this.updateTime();
    setInterval(() => {
      this.updateTime();
    }, 1000);
  }

  toggleD()
  {
    this.dashboard = false;
    this.devices = true;
  }
  toggleDa(){
    this.dashboard = true;
    this.devices = false;
  }

  updateTime() {
    this.currentTime = new Date();
  }
}
