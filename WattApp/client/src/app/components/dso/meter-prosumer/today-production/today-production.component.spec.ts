import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodayProductionComponent } from './today-production.component';

describe('TodayProductionComponent', () => {
  let component: TodayProductionComponent;
  let fixture: ComponentFixture<TodayProductionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TodayProductionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TodayProductionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
