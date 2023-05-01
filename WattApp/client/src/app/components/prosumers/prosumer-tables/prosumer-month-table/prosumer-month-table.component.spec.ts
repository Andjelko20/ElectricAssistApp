import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerMonthTableComponent } from './prosumer-month-table.component';

describe('ProsumerMonthTableComponent', () => {
  let component: ProsumerMonthTableComponent;
  let fixture: ComponentFixture<ProsumerMonthTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerMonthTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerMonthTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
