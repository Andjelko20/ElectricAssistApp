import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodayTabelarProsumerComponent } from './today-tabelar-prosumer.component';

describe('TodayTabelarProsumerComponent', () => {
  let component: TodayTabelarProsumerComponent;
  let fixture: ComponentFixture<TodayTabelarProsumerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TodayTabelarProsumerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TodayTabelarProsumerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
