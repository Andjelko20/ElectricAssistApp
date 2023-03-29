import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LineDayChartComponent } from './line-day-chart.component';

describe('LineDayChartComponent', () => {
  let component: LineDayChartComponent;
  let fixture: ComponentFixture<LineDayChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LineDayChartComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LineDayChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
