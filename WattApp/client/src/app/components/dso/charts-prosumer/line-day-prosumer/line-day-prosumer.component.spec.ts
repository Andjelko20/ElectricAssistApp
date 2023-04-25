import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LineDayProsumerComponent } from './line-day-prosumer.component';

describe('LineDayProsumerComponent', () => {
  let component: LineDayProsumerComponent;
  let fixture: ComponentFixture<LineDayProsumerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LineDayProsumerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LineDayProsumerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
