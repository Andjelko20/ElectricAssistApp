import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerDayTableComponent } from './prosumer-day-table.component';

describe('ProsumerDayTableComponent', () => {
  let component: ProsumerDayTableComponent;
  let fixture: ComponentFixture<ProsumerDayTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerDayTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerDayTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
