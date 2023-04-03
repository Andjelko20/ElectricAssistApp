import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerReportsPageComponent } from './prosumer-reports-page.component';

describe('ProsumerReportsPageComponent', () => {
  let component: ProsumerReportsPageComponent;
  let fixture: ComponentFixture<ProsumerReportsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerReportsPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerReportsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
