import { ComponentFixture, TestBed } from '@angular/core/testing';

import { YearTabelarProsumerComponent } from './year-tabelar-prosumer.component';

describe('YearTabelarProsumerComponent', () => {
  let component: YearTabelarProsumerComponent;
  let fixture: ComponentFixture<YearTabelarProsumerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ YearTabelarProsumerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(YearTabelarProsumerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
