import { Component, ElementRef, ViewChildren, QueryList, ViewChild} from '@angular/core';

@Component({
  selector: 'app-prosumer-reports-page',
  templateUrl: './prosumer-reports-page.component.html',
  styleUrls: ['./prosumer-reports-page.component.css']
})
export class ProsumerReportsPageComponent {
  
  @ViewChildren('collapsibleButton') collapsibleButtons!: QueryList<ElementRef>;
  dashboard:boolean = true;
  devices:boolean = false;
  day:boolean = true;
  week:boolean = false;
  month:boolean = false;
  year:boolean = false;
  constructor(private elementRef: ElementRef) {}
  dayTable:boolean = true;
  weekTable:boolean = false;
  monthTable:boolean = false;
  yearTable:boolean = false;
  toggleD()
  {
    this.dashboard = false;
    this.devices = true;
  }
  toggleDa(){
    this.dashboard = true;
    this.devices = false;
  }
  toogleDay()
  {
    this.day = true;
    this.week = false;
    this.month = false;
    this.year = false;
  }
  toogleWeek()
  {
    this.day = false;
    this.week = true;
    this.month = false;
    this.year = false;
  }
  toogleMonth()
  {
    this.day = false;
    this.week = false;
    this.month = true;
    this.year = false;
  }
  toogleYear()
  {
    this.day = false;
    this.week = false;
    this.month = false;
    this.year = true;
  }
  toogledayTable()
  {
    this.dayTable = true;
    this.weekTable = false;
    this.monthTable = false;
    this.yearTable = false;

  }
  toogleweekTable()
  {
    this.dayTable=false;
    this.weekTable=true;
    this.monthTable = false;
    this.yearTable = false;
  }
  tooglemonthTable()
  {
    this.dayTable=false;
    this.weekTable=false;
    this.monthTable=true;
    this.yearTable = false;
  }
  toogleyearTable()
  {
    this.dayTable=false;
    this.weekTable=false;
    this.monthTable=false;
    this.yearTable = true;
  }
  ngAfterViewInit() {
    this.collapsibleButtons.forEach(button => {
      button.nativeElement.addEventListener('click', () => {
        button.nativeElement.classList.toggle('active');
        const content = button.nativeElement.nextElementSibling;
        if (content.style.display === 'block') {
          content.style.display = 'none';
        } else {
          content.style.display = 'block';
        }
      });
    });
  }
}
