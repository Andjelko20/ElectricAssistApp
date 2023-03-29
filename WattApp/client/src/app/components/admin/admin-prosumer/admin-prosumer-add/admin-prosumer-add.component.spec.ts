import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminProsumerAddComponent } from './admin-prosumer-add.component';

describe('AdminProsumerAddComponent', () => {
  let component: AdminProsumerAddComponent;
  let fixture: ComponentFixture<AdminProsumerAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminProsumerAddComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminProsumerAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
