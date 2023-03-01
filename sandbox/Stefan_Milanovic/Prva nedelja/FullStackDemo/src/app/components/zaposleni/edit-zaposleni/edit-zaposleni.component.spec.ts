import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditZaposleniComponent } from './edit-zaposleni.component';

describe('EditZaposleniComponent', () => {
  let component: EditZaposleniComponent;
  let fixture: ComponentFixture<EditZaposleniComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditZaposleniComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditZaposleniComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
