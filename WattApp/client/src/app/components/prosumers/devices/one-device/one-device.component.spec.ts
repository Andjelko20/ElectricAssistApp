import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OneDeviceComponent } from './one-device.component';

describe('OneDeviceComponent', () => {
  let component: OneDeviceComponent;
  let fixture: ComponentFixture<OneDeviceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OneDeviceComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OneDeviceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
