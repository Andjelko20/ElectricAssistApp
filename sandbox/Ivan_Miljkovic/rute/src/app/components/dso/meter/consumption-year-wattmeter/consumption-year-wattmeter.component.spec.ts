import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsumptionYearWattmeterComponent } from './consumption-year-wattmeter.component';

describe('ConsumptionYearWattmeterComponent', () => {
  let component: ConsumptionYearWattmeterComponent;
  let fixture: ComponentFixture<ConsumptionYearWattmeterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConsumptionYearWattmeterComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConsumptionYearWattmeterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
