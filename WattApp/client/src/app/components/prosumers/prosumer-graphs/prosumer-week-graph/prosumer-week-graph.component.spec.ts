import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerWeekGraphComponent } from './prosumer-week-graph.component';

describe('ProsumerWeekGraphComponent', () => {
  let component: ProsumerWeekGraphComponent;
  let fixture: ComponentFixture<ProsumerWeekGraphComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerWeekGraphComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerWeekGraphComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
