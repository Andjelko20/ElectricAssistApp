import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeviceWeekComponent } from './device-week.component';

describe('DeviceWeekComponent', () => {
  let component: DeviceWeekComponent;
  let fixture: ComponentFixture<DeviceWeekComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeviceWeekComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeviceWeekComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
