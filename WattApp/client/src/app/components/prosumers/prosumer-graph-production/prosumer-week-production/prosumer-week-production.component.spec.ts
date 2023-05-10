import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerWeekProductionComponent } from './prosumer-week-production.component';

describe('ProsumerWeekProductionComponent', () => {
  let component: ProsumerWeekProductionComponent;
  let fixture: ComponentFixture<ProsumerWeekProductionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerWeekProductionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerWeekProductionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
