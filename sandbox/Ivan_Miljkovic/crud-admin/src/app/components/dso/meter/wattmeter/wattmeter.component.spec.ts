import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WattmeterComponent } from './wattmeter.component';

describe('WattmeterComponent', () => {
  let component: WattmeterComponent;
  let fixture: ComponentFixture<WattmeterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WattmeterComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WattmeterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
