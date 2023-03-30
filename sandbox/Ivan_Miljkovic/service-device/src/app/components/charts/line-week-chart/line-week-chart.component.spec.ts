import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LineWeekChartComponent } from './line-week-chart.component';

describe('LineWeekChartComponent', () => {
  let component: LineWeekChartComponent;
  let fixture: ComponentFixture<LineWeekChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LineWeekChartComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LineWeekChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
