import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeviceWeekTabularComponent } from './device-week-tabular.component';

describe('DeviceWeekTabularComponent', () => {
  let component: DeviceWeekTabularComponent;
  let fixture: ComponentFixture<DeviceWeekTabularComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeviceWeekTabularComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeviceWeekTabularComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
