import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminProsumerComponent } from './admin-prosumer.component';

describe('AdminProsumerComponent', () => {
  let component: AdminProsumerComponent;
  let fixture: ComponentFixture<AdminProsumerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminProsumerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminProsumerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
