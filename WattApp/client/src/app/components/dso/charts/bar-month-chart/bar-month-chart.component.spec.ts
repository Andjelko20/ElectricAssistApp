import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BarMonthChartComponent } from './bar-month-chart.component';

describe('BarMonthChartComponent', () => {
  let component: BarMonthChartComponent;
  let fixture: ComponentFixture<BarMonthChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BarMonthChartComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BarMonthChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
