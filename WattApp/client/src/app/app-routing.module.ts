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
import { MapsComponent } from './components/maps/maps.component';

import { DsoHomePageComponent } from './pages/dso/dso-home-page/dso-home-page.component';
import { DsoProsumersPageComponent } from './pages/dso/dso-prosumers-page/dso-prosumers-page.component';
import { AdminProsumersPageComponent } from './pages/dso/admin/admin-prosumers-page/admin-prosumers-page.component';
import { ForgotPasswordPageComponent } from './pages/forgot-password/forgot-password.component';
import { ResetPasswordPageComponent } from './pages/reset-password/reset-password.component';
import { ProsumersMapComponent } from './components/prosumers-map/prosumers-map.component';
import { DispatcherGuard } from './guards/dispatcher.guard';
 
const routes: Routes = [
	{path:'',redirectTo:'home',pathMatch:'full'},
	{path:'login',component:LoginComponent,canActivate:[UnauthenticatedGuard]},
	{path:'home',component:HomeComponent,canActivate:[AuthenticatedGuard]},
	{path:'add-comp',component:AddCompComponent,canActivate:[AuthenticatedGuard]},
	{path:'update/:id',component:UpdateComponent,canActivate:[AuthenticatedGuard]},
	{path:'register',component:RegisterComponent},
	{path:'maps',component:MapsComponent},
	{path:'change-password',component:ChangePasswordComponent},
	{path:'dsohome',component:DsoHomePageComponent,canActivate:[AuthenticatedGuard]},
	{path:'dsoprosumer',component:DsoProsumersPageComponent,canActivate:[AuthenticatedGuard]},
	{path:'admin-prosumers-page',component:AdminProsumersPageComponent,canActivate:[AuthenticatedGuard]},
	{path:'forgot-password',component:ForgotPasswordPageComponent,canActivate:[UnauthenticatedGuard]},
	{path:'reset-password',component:ResetPasswordPageComponent},
	{path:"prosumer-map",component:ProsumersMapComponent,canActivate:[DispatcherGuard]},
	{path:'**',redirectTo:"login"}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
