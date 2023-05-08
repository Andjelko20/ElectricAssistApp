import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeviceMonthTabularComponent } from './device-month-tabular.component';

describe('DeviceMonthTabularComponent', () => {
  let component: DeviceMonthTabularComponent;
  let fixture: ComponentFixture<DeviceMonthTabularComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeviceMonthTabularComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeviceMonthTabularComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
