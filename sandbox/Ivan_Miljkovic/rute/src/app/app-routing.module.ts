import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { UnauthenticatedGuard } from './guards/unauthenticated.guard';
import { AuthenticatedGuard } from './guards/authenticated.guard';

import { FutureComponent } from './components/future/future.component';
import { DsoHomePageComponent } from './pages/dso/dso-home-page/dso-home-page.component';
import { DsoProsumersPageComponent } from './pages/dso/dso-prosumers-page/dso-prosumers-page.component';
import { ForgotPasswordPageComponent } from './pages/forgot-password/forgot-password.component';
import { ResetPasswordPageComponent } from './pages/reset-password/reset-password.component';
import { ProsumerHomePageComponent } from './pages/prosumer/prosumer-home-page/prosumer-home-page.component';
import { AdminDsoComponent } from './components/admin/admin-dso/admin-dso.component';
import { AdminDsoUpdateComponent } from './components/admin/admin-dso-update/admin-dso-update.component';
import { AdminDsoAddComponent } from './components/admin/admin-dso-add/admin-dso-add.component';

import { ProsumersMapComponent } from './components/prosumers-map/prosumers-map.component';
import { DispatcherGuard } from './guards/dispatcher.guard';
import { MapInputComponent } from './components/map-input/map-input.component';
import { AllDevicesComponent } from './components/prosumers/devices/all-devices/all-devices.component';
import { AddDeviceComponent } from './components/prosumers/devices/add-device/add-device.component';
import { UpdateDeviceComponent } from './components/prosumers/devices/update-device/update-device.component';
import { AdminGuard } from './guards/admin.guard';
import { ProsumerGuard } from './guards/prosumer.guard';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { AdminDsoPageComponent } from './pages/admin/admin-dso-page/admin-dso-page.component';

 
const routes: Routes = [
	{path:'',redirectTo:'home',pathMatch:'full'},
	{path:'login',component:LoginComponent,canActivate:[UnauthenticatedGuard]},
	{path:'forgot-password',component:ForgotPasswordPageComponent,canActivate:[UnauthenticatedGuard]},

	{path:'reset-password/:id',component:ResetPasswordPageComponent,canActivate:[UnauthenticatedGuard]},
	
	{path:'home',component:HomePageComponent,canActivate:[AuthenticatedGuard],
		children:[
		{path:'admin',component:AdminDsoPageComponent,canActivate:[AdminGuard]
			,children:[
				{path:'',component:AdminDsoComponent,canActivate:[AdminGuard]},
				{path:'add',component:AdminDsoAddComponent,canActivate:[AdminGuard]},
				{path:'update/:id',component:AdminDsoUpdateComponent,canActivate:[AdminGuard]}
			]}
			
		,{path:'dso',component:DsoHomePageComponent,canActivate:[DispatcherGuard]},
		{path:'prosumer',component:ProsumerHomePageComponent,canActivate:[ProsumerGuard]},
	]},
	
	// {path:'admin',component:AdminDsoComponent,canActivate:[AdminGuard]},
	// {path:'add',component:AdminDsoAddComponent,canActivate:[AdminGuard]},
	// {path:'update/:id',component:AdminDsoUpdateComponent,canActivate:[AdminGuard]},
	//{path:'register',component:RegisterComponent},
	//{path:'change-password',component:ChangePasswordComponent},
	// {path:'dsohome',component:DsoHomePageComponent,canActivate:[DispatcherGuard]},
	{path:'dsoprosumer',component:DsoProsumersPageComponent,canActivate:[DispatcherGuard]},
	// {path:'prosumer-home-page',component:ProsumerHomePageComponent,canActivate:[ProsumerGuard]},
	{path:'reset-password',component:ResetPasswordPageComponent},
	{path:"prosumer-map",component:ProsumersMapComponent},//canActivate:[DispatcherGuard]},
	{path:"map-input",component:MapInputComponent},
	{path:'future',component:FutureComponent},
	{path:"devices",component:AllDevicesComponent},
	{path:"devices-add",component:AddDeviceComponent},
	{path:"devices-update/:id",component:UpdateDeviceComponent},
	{path:'**',redirectTo:"login"}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }