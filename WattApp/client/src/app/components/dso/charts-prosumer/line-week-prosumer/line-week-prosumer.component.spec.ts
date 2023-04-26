import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LineWeekProsumerComponent } from './line-week-prosumer.component';

describe('LineWeekProsumerComponent', () => {
  let component: LineWeekProsumerComponent;
  let fixture: ComponentFixture<LineWeekProsumerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LineWeekProsumerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LineWeekProsumerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
