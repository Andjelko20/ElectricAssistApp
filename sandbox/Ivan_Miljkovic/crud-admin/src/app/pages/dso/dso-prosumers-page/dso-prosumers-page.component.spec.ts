import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DsoProsumersPageComponent } from './dso-prosumers-page.component';

describe('DsoProsumersPageComponent', () => {
  let component: DsoProsumersPageComponent;
  let fixture: ComponentFixture<DsoProsumersPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DsoProsumersPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DsoProsumersPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
