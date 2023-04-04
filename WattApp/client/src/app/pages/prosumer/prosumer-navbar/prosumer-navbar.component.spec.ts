import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerNavbarComponent } from './prosumer-navbar.component';

describe('ProsumerNavbarComponent', () => {
  let component: ProsumerNavbarComponent;
  let fixture: ComponentFixture<ProsumerNavbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerNavbarComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerNavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
