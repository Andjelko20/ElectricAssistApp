import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DriverDetailsComponent } from './components/driver-details/driver-details.component';
import { DriverEditComponent } from './components/driver-edit/driver-edit.component';
import { DriverListComponent } from './components/driver-list/driver-list.component';
import { HomeComponent } from './components/home/home.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  {
    path: 'drivers',
    component: DriverListComponent
  },
  {
    path: 'drivers/:id',
    component: DriverDetailsComponent
  },
  {
    path: 'drivers/:id/edit',
    component: DriverEditComponent
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
