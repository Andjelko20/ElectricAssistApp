import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class UnsavedChangesGuardGuard implements CanDeactivate<unknown> {
  canDeactivate(component: any): boolean {
    if (component.isFormDirty || component.isFormDirty1) {
      component.body="Are you sure you want to leave? Your unsaved changes will be lost."
      component.btnAction="Confirm"
      component.confirm=true;
      return component.modalService.open(component.modalContent).result
      .then((result: any) => {
        
        if (result === 'Save click') {
          return true; 
        } else {
          return false; 
        }
      })
      .catch(() => {
        return false; 
      });
      
    }
    return true;
  }
  
}
