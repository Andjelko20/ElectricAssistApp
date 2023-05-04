import { Component, OnInit } from '@angular/core';
import { pluck } from 'rxjs';
import { ForecastService } from 'src/app/services/forecast.service';


@Component({
  selector: 'app-future',
  templateUrl: './future.component.html',
  styleUrls: ['./future.component.css']
})
export class FutureComponent implements OnInit{
  weatherData: any = [];
  forecastDetails:any
  primaryDisplay = true
  secondaryDisplay = false
  selectedIndex!: number;
  location:any
  constructor(private forecastService: ForecastService) { }

  ngOnInit(): void {
    this.forecastService.getWeatherForecast().subscribe(data=>{
      this.getTodayForecast(data)
    })
    
    this.forecastService.getWeatherForecast().pipe(
      pluck('list')
    )
    .subscribe((data: unknown) => {
      this.futureForecast(data);
    });
  }

  getTodayForecast(today:any) {
    this.location = today.city;
  }
  futureForecast(data: any){
    if(!data) return;
    for(let i = 0 ; i < data.length ; i += 8) {
      this.weatherData.push(data[i]);
    }
  }

  toggle(data:any,index:number){
    this.primaryDisplay = !this.primaryDisplay;
    this.secondaryDisplay = !this.secondaryDisplay;

    this.forecastDetails = data;
    this.selectedIndex = index;
  }
  toggles(){
    this.primaryDisplay = !this.primaryDisplay;
    this.secondaryDisplay = !this.secondaryDisplay;
  }

  showDetails(data:any,i:number){
    this.forecastDetails = data;
    this.selectedIndex = i;
  }
}
