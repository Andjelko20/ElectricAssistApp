import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BarMonthProsumerComponent } from './bar-month-prosumer.component';

describe('BarMonthProsumerComponent', () => {
  let component: BarMonthProsumerComponent;
  let fixture: ComponentFixture<BarMonthProsumerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BarMonthProsumerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BarMonthProsumerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
