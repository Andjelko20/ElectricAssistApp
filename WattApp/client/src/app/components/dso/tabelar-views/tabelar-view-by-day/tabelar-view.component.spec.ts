import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TabelarViewComponent } from './tabelar-view.component';

describe('TabelarViewComponent', () => {
  let component: TabelarViewComponent;
  let fixture: ComponentFixture<TabelarViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TabelarViewComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TabelarViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
