import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PredictionDsoComponent } from './prediction-dso.component';

describe('PredictionDsoComponent', () => {
  let component: PredictionDsoComponent;
  let fixture: ComponentFixture<PredictionDsoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PredictionDsoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PredictionDsoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
