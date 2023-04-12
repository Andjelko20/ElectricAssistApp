import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerAccountSettingsPageComponent } from './prosumer-account-settings-page.component';

describe('ProsumerAccountSettingsPageComponent', () => {
  let component: ProsumerAccountSettingsPageComponent;
  let fixture: ComponentFixture<ProsumerAccountSettingsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerAccountSettingsPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerAccountSettingsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
