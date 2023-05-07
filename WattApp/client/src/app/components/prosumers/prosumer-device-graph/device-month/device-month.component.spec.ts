import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeviceMonthComponent } from './device-month.component';

describe('DeviceMonthComponent', () => {
  let component: DeviceMonthComponent;
  let fixture: ComponentFixture<DeviceMonthComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeviceMonthComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeviceMonthComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
