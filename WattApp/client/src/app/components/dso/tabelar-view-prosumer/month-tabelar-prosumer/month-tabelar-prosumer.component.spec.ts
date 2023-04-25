import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthTabelarProsumerComponent } from './month-tabelar-prosumer.component';

describe('MonthTabelarProsumerComponent', () => {
  let component: MonthTabelarProsumerComponent;
  let fixture: ComponentFixture<MonthTabelarProsumerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MonthTabelarProsumerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MonthTabelarProsumerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
