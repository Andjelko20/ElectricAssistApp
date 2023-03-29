import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DsoPredictionPageComponent } from './dso-prediction-page.component';

describe('DsoPredictionPageComponent', () => {
  let component: DsoPredictionPageComponent;
  let fixture: ComponentFixture<DsoPredictionPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DsoPredictionPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DsoPredictionPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
