import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { UnauthenticatedGuard } from './guards/unauthenticated.guard';
import { AuthenticatedGuard } from './guards/authenticated.guard';
import { AddCompComponent } from './components/add-comp/add-comp.component';
import { UpdateComponent } from './components/update/update.component';
import { RegisterComponent } from './pages/register/register.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { ChartsComponent } from './pages/dso/charts/charts.component';
import { WattmeterComponent } from './components/wattmeter/wattmeter.component';
import { DsoHomePageComponent } from './pages/dso/dso-home-page/dso-home-page.component';
import { Wattmeter1Component } from './components/wattmeter1/wattmeter1.component';


 
const routes: Routes = [
	{path:'',redirectTo:'home',pathMatch:'full'},
	{path:'login',component:LoginComponent,canActivate:[UnauthenticatedGuard]},
	{path:'home',component:HomeComponent,canActivate:[AuthenticatedGuard]},
	{path:'add-comp',component:AddCompComponent,canActivate:[AuthenticatedGuard]},
	{path:'update/:id',component:UpdateComponent,canActivate:[AuthenticatedGuard]},
	{path:'register',component:RegisterComponent},
	{path:'change-password',component:ChangePasswordComponent},
	{path:'meter',component:WattmeterComponent},
	{path:'charts',component:ChartsComponent},
	{path:'dso',component:DsoHomePageComponent},
	{path:'wattmeter',component:WattmeterComponent},
	{path:'wattmeter1',component:Wattmeter1Component},
	{path:'**',redirectTo:"login"}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
