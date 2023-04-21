import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TabelarViewByYearComponent } from './tabelar-view-by-year.component';

describe('TabelarViewByYearComponent', () => {
  let component: TabelarViewByYearComponent;
  let fixture: ComponentFixture<TabelarViewByYearComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TabelarViewByYearComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TabelarViewByYearComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
