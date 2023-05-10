import { TestBed } from '@angular/core/testing';

import { EmailConfirmationServiceService } from './email-confirmation-service.service';

describe('EmailConfirmationServiceService', () => {
  let service: EmailConfirmationServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EmailConfirmationServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
