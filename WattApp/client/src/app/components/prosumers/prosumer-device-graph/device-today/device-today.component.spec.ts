import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeviceTodayComponent } from './device-today.component';

describe('DeviceTodayComponent', () => {
  let component: DeviceTodayComponent;
  let fixture: ComponentFixture<DeviceTodayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeviceTodayComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeviceTodayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
