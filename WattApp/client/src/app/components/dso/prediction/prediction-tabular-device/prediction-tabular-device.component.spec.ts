import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PredictionTabularDeviceComponent } from './prediction-tabular-device.component';

describe('PredictionTabularDeviceComponent', () => {
  let component: PredictionTabularDeviceComponent;
  let fixture: ComponentFixture<PredictionTabularDeviceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PredictionTabularDeviceComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PredictionTabularDeviceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
