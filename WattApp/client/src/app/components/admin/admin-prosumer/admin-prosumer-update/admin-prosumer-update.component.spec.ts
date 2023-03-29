import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminProsumerUpdateComponent } from './admin-prosumer-update.component';

describe('AdminProsumerUpdateComponent', () => {
  let component: AdminProsumerUpdateComponent;
  let fixture: ComponentFixture<AdminProsumerUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminProsumerUpdateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminProsumerUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
