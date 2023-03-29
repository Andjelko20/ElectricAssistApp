import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsumptionMonthWattmeterComponent } from './consumption-month-wattmeter.component';

describe('ConsumptionMonthWattmeterComponent', () => {
  let component: ConsumptionMonthWattmeterComponent;
  let fixture: ComponentFixture<ConsumptionMonthWattmeterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConsumptionMonthWattmeterComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConsumptionMonthWattmeterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
