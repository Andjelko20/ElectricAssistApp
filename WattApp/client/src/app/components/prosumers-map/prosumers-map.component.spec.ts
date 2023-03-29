import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumersMapComponent } from './prosumers-map.component';

describe('ProsumersMapComponent', () => {
  let component: ProsumersMapComponent;
  let fixture: ComponentFixture<ProsumersMapComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumersMapComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumersMapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
