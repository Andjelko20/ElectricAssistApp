import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerMonthGraphComponent } from './prosumer-month-graph.component';

describe('ProsumerMonthGraphComponent', () => {
  let component: ProsumerMonthGraphComponent;
  let fixture: ComponentFixture<ProsumerMonthGraphComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerMonthGraphComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerMonthGraphComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
