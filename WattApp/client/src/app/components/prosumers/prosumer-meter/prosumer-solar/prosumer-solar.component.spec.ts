import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerSolarComponent } from './prosumer-solar.component';

describe('ProsumerSolarComponent', () => {
  let component: ProsumerSolarComponent;
  let fixture: ComponentFixture<ProsumerSolarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerSolarComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerSolarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
