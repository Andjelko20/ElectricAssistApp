import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerFooterComponent } from './prosumer-footer.component';

describe('ProsumerFooterComponent', () => {
  let component: ProsumerFooterComponent;
  let fixture: ComponentFixture<ProsumerFooterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerFooterComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerFooterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
