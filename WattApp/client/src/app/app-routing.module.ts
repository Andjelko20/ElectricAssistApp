import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { UnauthenticatedGuard } from './guards/unauthenticated.guard';
import { AuthenticatedGuard } from './guards/authenticated.guard';
import { AddCompComponent } from './pages/add-comp/add-comp.component';
import { UpdateComponent } from './pages/update/update.component';

const routes: Routes = [
	{path:'',redirectTo:'home',pathMatch:'full'},
	{path:'login',component:LoginComponent,canActivate:[UnauthenticatedGuard]},
	{path:'home',component:HomeComponent,canActivate:[AuthenticatedGuard]},
	{path:'add-comp',component:AddCompComponent,canActivate:[AuthenticatedGuard]},
	{path:'update/:id',component:UpdateComponent,canActivate:[AuthenticatedGuard]},
	{path:'**',redirectTo:"login"}
	
	
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
