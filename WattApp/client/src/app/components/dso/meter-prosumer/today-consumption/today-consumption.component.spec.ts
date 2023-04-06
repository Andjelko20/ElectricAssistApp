import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodayConsumptionComponent } from './today-consumption.component';

describe('TodayConsumptionComponent', () => {
  let component: TodayConsumptionComponent;
  let fixture: ComponentFixture<TodayConsumptionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TodayConsumptionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TodayConsumptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
