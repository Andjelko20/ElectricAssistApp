import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsumptionTodayWattmeterComponent } from './consumption-today-wattmeter.component';

describe('ConsumptionTodayWattmeterComponent', () => {
  let component: ConsumptionTodayWattmeterComponent;
  let fixture: ComponentFixture<ConsumptionTodayWattmeterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConsumptionTodayWattmeterComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConsumptionTodayWattmeterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
