import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddEmployeeComponent } from './commponents/empoyees/add-employee/add-employee.component';
import { EditEmployeeComponent } from './commponents/empoyees/edit-employee/edit-employee.component';
import { EmployeesListComponent } from './commponents/empoyees/employees-list/employees-list.component';

const routes: Routes = [
  {
    path:'',
    component:EmployeesListComponent
  },
  {
    path:'employees',
    component:EmployeesListComponent
  },
  {
    path:'employees/add',
    component:AddEmployeeComponent
  },
  {
    path:'employees/edit/:id',
    component:EditEmployeeComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
