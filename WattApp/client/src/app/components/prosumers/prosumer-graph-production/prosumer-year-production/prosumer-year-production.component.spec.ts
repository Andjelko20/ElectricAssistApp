import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerYearProductionComponent } from './prosumer-year-production.component';

describe('ProsumerYearProductionComponent', () => {
  let component: ProsumerYearProductionComponent;
  let fixture: ComponentFixture<ProsumerYearProductionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerYearProductionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerYearProductionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
