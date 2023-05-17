import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AdminDsoAddComponent } from '../components/admin/admin-dso-add/admin-dso-add.component';

@Injectable({
  providedIn: 'root'
})
export class UnsavedChangesGuardGuard implements CanDeactivate<unknown> {
  canDeactivate(component: any): boolean {
    if (component.isFormDirty || component.isFormDirty1) {
      return confirm('Are you sure you want to leave? Your unsaved changes will be lost.');
    }
    return true;
  }
  
}
