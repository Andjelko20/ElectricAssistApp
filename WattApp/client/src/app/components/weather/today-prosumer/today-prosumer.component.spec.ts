import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodayProsumerComponent } from './today-prosumer.component';

describe('TodayProsumerComponent', () => {
  let component: TodayProsumerComponent;
  let fixture: ComponentFixture<TodayProsumerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TodayProsumerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TodayProsumerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
