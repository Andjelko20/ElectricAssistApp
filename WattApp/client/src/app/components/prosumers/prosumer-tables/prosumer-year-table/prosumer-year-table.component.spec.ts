import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerYearTableComponent } from './prosumer-year-table.component';

describe('ProsumerYearTableComponent', () => {
  let component: ProsumerYearTableComponent;
  let fixture: ComponentFixture<ProsumerYearTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerYearTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerYearTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
