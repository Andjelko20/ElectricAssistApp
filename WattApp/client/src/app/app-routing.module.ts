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
import { DsoOneProsumerDevicesPageComponent } from './pages/dso/dso-one-prosumer-devices-page/dso-one-prosumer-devices-page.component';
import { DsoOneProsumerPageComponent } from './pages/dso/dso-one-prosumer-page/dso-one-prosumer-page.component';
import { DsoPredictionPageComponent } from './pages/dso/dso-prediction-page/dso-prediction-page.component';
import { ProsumerDevicesPageComponent } from './pages/prosumer/prosumer-devices-page/prosumer-devices-page.component';
import { ProsumerReportsPageComponent } from './pages/prosumer/prosumer-reports-page/prosumer-reports-page.component';
import { ProsumerOneDevicePageComponent } from './pages/prosumer/prosumer-one-device-page/prosumer-one-device-page.component';

 
const routes: Routes = [
	{path:'',redirectTo:'dashboard',pathMatch:'full'},
	//login
	{path:'login',component:LoginComponent,canActivate:[UnauthenticatedGuard]},
	{path:'forgot-password',component:ForgotPasswordPageComponent,canActivate:[UnauthenticatedGuard]},
	{path:'reset-password/:id',component:ResetPasswordPageComponent,canActivate:[UnauthenticatedGuard]},
	//home
	{path:'dashboard',component:HomePageComponent,canActivate:[AuthenticatedGuard]},
	//ADMIN
	{path:'',component:AdminDsoPageComponent,canActivate:[AdminGuard]},
	{path:'add',component:AdminDsoAddComponent,canActivate:[AdminGuard]},
	{path:'update/:id',component:AdminDsoUpdateComponent,canActivate:[AdminGuard]},
	//DSO
	{path:'',component:DsoHomePageComponent,canActivate:[DispatcherGuard]},
	{path:'prosumers',component:DsoProsumersPageComponent,canActivate:[DispatcherGuard]},
	{path:'one-prosumer',component:DsoOneProsumerPageComponent,canActivate:[DispatcherGuard]},
	{path:'one-prosumer-devices',component:DsoOneProsumerDevicesPageComponent,canActivate:[DispatcherGuard]},
	{path:'prediction',component:DsoPredictionPageComponent,canActivate:[DispatcherGuard]},
	//PROSUMER
	{path:'',component:ProsumerHomePageComponent,canActivate:[ProsumerGuard]},
	{path:'devices',component:ProsumerDevicesPageComponent,canActivate:[ProsumerGuard]},
	{path:'reports',component:ProsumerReportsPageComponent,canActivate:[ProsumerGuard]},
	{path:'one-device',component:ProsumerOneDevicePageComponent,canActivate:[ProsumerGuard]},
	
	//TEST
	// {path:'register',component:RegisterComponent},
	// {path:'change-password',component:ChangePasswordComponent},
	
	//{path:'reset-password',component:ResetPasswordPageComponent},
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
