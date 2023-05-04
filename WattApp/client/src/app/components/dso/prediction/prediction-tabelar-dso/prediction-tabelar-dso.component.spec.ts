import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PredictionTabelarDsoComponent } from './prediction-tabelar-dso.component';

describe('PredictionTabelarDsoComponent', () => {
  let component: PredictionTabelarDsoComponent;
  let fixture: ComponentFixture<PredictionTabelarDsoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PredictionTabelarDsoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PredictionTabelarDsoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
