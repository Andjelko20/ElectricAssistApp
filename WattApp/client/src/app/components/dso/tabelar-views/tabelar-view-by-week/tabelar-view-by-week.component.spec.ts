import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TabelarViewByWeekComponent } from './tabelar-view-by-week.component';

describe('TabelarViewByWeekComponent', () => {
  let component: TabelarViewByWeekComponent;
  let fixture: ComponentFixture<TabelarViewByWeekComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TabelarViewByWeekComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TabelarViewByWeekComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
