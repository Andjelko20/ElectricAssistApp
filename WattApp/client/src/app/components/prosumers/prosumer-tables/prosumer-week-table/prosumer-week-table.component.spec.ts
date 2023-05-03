import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerWeekTableComponent } from './prosumer-week-table.component';

describe('ProsumerWeekTableComponent', () => {
  let component: ProsumerWeekTableComponent;
  let fixture: ComponentFixture<ProsumerWeekTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerWeekTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerWeekTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
