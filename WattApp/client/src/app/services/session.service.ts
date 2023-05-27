import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SessionService {
  private readonly SESSION_PREFIX = "Session_";

  constructor() { }

  setSession(name: string, data: any): void {
    const sessionKey = this.SESSION_PREFIX + name;
    sessionStorage.setItem(sessionKey, JSON.stringify(data));
  }

  getSession(name: string): any {
    const sessionKey = this.SESSION_PREFIX + name;
    const sessionData = sessionStorage.getItem(sessionKey);
    return sessionData ? JSON.parse(sessionData) : null;
  }

  clearSession(name: string): void {
    const sessionKey = this.SESSION_PREFIX + name;
    sessionStorage.removeItem(sessionKey);
  }
}
