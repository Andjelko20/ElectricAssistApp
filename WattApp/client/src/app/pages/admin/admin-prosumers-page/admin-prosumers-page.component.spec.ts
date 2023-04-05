import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminProsumersPageComponent } from './admin-prosumers-page.component';

describe('AdminProsumersPageComponent', () => {
  let component: AdminProsumersPageComponent;
  let fixture: ComponentFixture<AdminProsumersPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminProsumersPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminProsumersPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
