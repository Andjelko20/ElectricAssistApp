import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrentPowerWattmeterComponent } from './current-power-wattmeter.component';

describe('CurrentPowerWattmeterComponent', () => {
  let component: CurrentPowerWattmeterComponent;
  let fixture: ComponentFixture<CurrentPowerWattmeterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CurrentPowerWattmeterComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CurrentPowerWattmeterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
