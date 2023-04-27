import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TabelarViewByMonthComponent } from './tabelar-view-by-month.component';

describe('TabelarViewByMonthComponent', () => {
  let component: TabelarViewByMonthComponent;
  let fixture: ComponentFixture<TabelarViewByMonthComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TabelarViewByMonthComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TabelarViewByMonthComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
