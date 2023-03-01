import { TestBed } from '@angular/core/testing';

import { ZaposleniService } from './zaposleni.service';

describe('ZaposleniService', () => {
  let service: ZaposleniService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ZaposleniService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
