import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeviceYearTabularComponent } from './device-year-tabular.component';

describe('DeviceYearTabularComponent', () => {
  let component: DeviceYearTabularComponent;
  let fixture: ComponentFixture<DeviceYearTabularComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeviceYearTabularComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeviceYearTabularComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
