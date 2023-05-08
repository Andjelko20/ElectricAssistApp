import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PredictionTabularProsumerComponent } from './prediction-tabular-prosumer.component';

describe('PredictionTabularProsumerComponent', () => {
  let component: PredictionTabularProsumerComponent;
  let fixture: ComponentFixture<PredictionTabularProsumerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PredictionTabularProsumerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PredictionTabularProsumerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
