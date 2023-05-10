import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeviceYearComponent } from './device-year.component';

describe('DeviceYearComponent', () => {
  let component: DeviceYearComponent;
  let fixture: ComponentFixture<DeviceYearComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeviceYearComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeviceYearComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
