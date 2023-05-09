import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerMonthProductionComponent } from './prosumer-month-production.component';

describe('ProsumerMonthProductionComponent', () => {
  let component: ProsumerMonthProductionComponent;
  let fixture: ComponentFixture<ProsumerMonthProductionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerMonthProductionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerMonthProductionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
