import {  Component, ElementRef, ViewChildren, QueryList, ViewChild } from '@angular/core';

@Component({
  selector: 'app-prosumer-device-page',
  templateUrl: './prosumer-device-page.component.html',
  styleUrls: ['./prosumer-device-page.component.css']
})
export class ProsumerDevicePageComponent {
  @ViewChildren('collapsibleButton') collapsibleButtons!: QueryList<ElementRef>;

  constructor(private elementRef: ElementRef) {}

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
