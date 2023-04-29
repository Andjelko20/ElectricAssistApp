import { Component, OnInit } from '@angular/core';
import { ForecastService } from 'src/app/services/forecast.service';

interface ForecastTimeline{
  time:any,
  temp:any
}

@Component({
  selector: 'app-today-dso',
  templateUrl: './today-dso.component.html',
  styleUrls: ['./today-dso.component.css']
})
export class TodayDsoComponent implements OnInit {
  cels:boolean = true
  fer:boolean = false
  timeline : ForecastTimeline[] = []
  weatherNow:any
  currentTime = new Date();
  location:any
  constructor(private forecastService: ForecastService) {
    
  }

  ngOnInit(): void {
    this.forecastService.getWeatherForecast().subscribe(data=>{
      this.getTodayForecast(data)
  })

  }  

  dateRange(){
    const start = new Date();
    start.setHours(start.getHours()+(start.getTimezoneOffset()/60));
    const to = new Date(start);

    to.setHours(to.getHours()+ 2,to.getMinutes()+ 59, to.getSeconds()+ 59);
  
    return { start , to }
  }

  getTodayForecast(today:any) {
    this.location = today.city;
    
    for(const forecast of today.list.slice(0,8)){
      this.timeline.push({
        time: forecast.dt_txt,
        temp : forecast.main.temp
      });
      const apiDate = new Date(forecast.dt_txt).getTime();
      if(this.dateRange().start.getTime() <= apiDate && this.dateRange().to.getTime() >= apiDate){
        this.weatherNow = forecast;
        console.log(this.weatherNow)
      }
    }
  }
}
