import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeviceTodayTabularComponent } from './device-today-tabular.component';

describe('DeviceTodayTabularComponent', () => {
  let component: DeviceTodayTabularComponent;
  let fixture: ComponentFixture<DeviceTodayTabularComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeviceTodayTabularComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeviceTodayTabularComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
