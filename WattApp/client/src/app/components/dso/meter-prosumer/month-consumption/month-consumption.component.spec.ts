import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthConsumptionComponent } from './month-consumption.component';

describe('MonthConsumptionComponent', () => {
  let component: MonthConsumptionComponent;
  let fixture: ComponentFixture<MonthConsumptionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MonthConsumptionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MonthConsumptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
