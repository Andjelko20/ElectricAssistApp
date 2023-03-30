import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BarYearChartComponent } from './bar-year-chart.component';

describe('BarYearChartComponent', () => {
  let component: BarYearChartComponent;
  let fixture: ComponentFixture<BarYearChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BarYearChartComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BarYearChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
