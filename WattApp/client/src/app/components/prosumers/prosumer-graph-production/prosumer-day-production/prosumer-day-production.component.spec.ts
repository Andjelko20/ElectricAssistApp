import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerDayProductionComponent } from './prosumer-day-production.component';

describe('ProsumerDayProductionComponent', () => {
  let component: ProsumerDayProductionComponent;
  let fixture: ComponentFixture<ProsumerDayProductionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerDayProductionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerDayProductionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
