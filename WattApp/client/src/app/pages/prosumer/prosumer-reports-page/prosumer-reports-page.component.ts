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
  constructor(private elementRef: ElementRef) {}

  toggleD()
  {
    this.dashboard = false;
    this.devices = true;
  }
  toggleDa(){
    this.dashboard = true;
    this.devices = false;
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
