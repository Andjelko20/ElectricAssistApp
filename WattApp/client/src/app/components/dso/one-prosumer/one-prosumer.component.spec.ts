import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OneProsumerComponent } from './one-prosumer.component';

describe('OneProsumerComponent', () => {
  let component: OneProsumerComponent;
  let fixture: ComponentFixture<OneProsumerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OneProsumerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OneProsumerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
