import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthProductionComponent } from './month-production.component';

describe('MonthProductionComponent', () => {
  let component: MonthProductionComponent;
  let fixture: ComponentFixture<MonthProductionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MonthProductionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MonthProductionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
