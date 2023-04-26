import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PredictionDeviceComponent } from './prediction-device.component';

describe('PredictionDeviceComponent', () => {
  let component: PredictionDeviceComponent;
  let fixture: ComponentFixture<PredictionDeviceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PredictionDeviceComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PredictionDeviceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
