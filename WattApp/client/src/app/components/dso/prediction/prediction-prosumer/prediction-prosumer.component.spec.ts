import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PredictionProsumerComponent } from './prediction-prosumer.component';

describe('PredictionProsumerComponent', () => {
  let component: PredictionProsumerComponent;
  let fixture: ComponentFixture<PredictionProsumerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PredictionProsumerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PredictionProsumerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
