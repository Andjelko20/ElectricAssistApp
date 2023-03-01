import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditFilmComponent } from './edit-film.component';

describe('EditFilmComponent', () => {
  let component: EditFilmComponent;
  let fixture: ComponentFixture<EditFilmComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditFilmComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditFilmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
