import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WeekTabelarProsumerComponent } from './week-tabelar-prosumer.component';

describe('WeekTabelarProsumerComponent', () => {
  let component: WeekTabelarProsumerComponent;
  let fixture: ComponentFixture<WeekTabelarProsumerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WeekTabelarProsumerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WeekTabelarProsumerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
