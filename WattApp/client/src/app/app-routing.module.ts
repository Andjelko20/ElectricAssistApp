import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { UnauthenticatedGuard } from './guards/unauthenticated.guard';
import { AuthenticatedGuard } from './guards/authenticated.guard';
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
import { AdminGuard } from './guards/admin.guard';
import { ProsumerGuard } from './guards/prosumer.guard';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { DsoPredictionPageComponent } from './pages/dso/dso-prediction-page/dso-prediction-page.component';
import { ProsumerDevicesPageComponent } from './pages/prosumer/prosumer-devices-page/prosumer-devices-page.component';
import { ProsumerReportsPageComponent } from './pages/prosumer/prosumer-reports-page/prosumer-reports-page.component';
import { OneProsumerComponent } from './components/dso/one-prosumer/one-prosumer.component';
import { ProducingConsumingComponent } from './components/dso/producing-consuming/producing-consuming.component';



import { TodayComponent } from './components/today/today.component';
import { ProsumerTowerComponent } from './components/prosumer-meter/prosumer-tower/prosumer-tower.component';
import { ProsumerSolarComponent } from './components/prosumer-meter/prosumer-solar/prosumer-solar.component';
import { ProsumerHouseComponent } from './components/prosumer-meter/prosumer-house/prosumer-house.component';
import { MapsComponent } from './components/dso/maps/maps.component';
import { ProsumerDevicePageComponent } from './pages/prosumer/prosumer-device-page/prosumer-device-page.component';
 
const routes: Routes = [
	{path:'',redirectTo:'dashboard',pathMatch:'full'},
	//login
	{path:'login',component:LoginComponent,canActivate:[UnauthenticatedGuard]},
	{path:'forgot-password',component:ForgotPasswordPageComponent,canActivate:[UnauthenticatedGuard]},
	{path:'reset-password/:id',component:ResetPasswordPageComponent,canActivate:[UnauthenticatedGuard]},
	//home
	{path:'dashboard',component:HomePageComponent,canActivate:[AuthenticatedGuard]},
	//ADMIN
	{path:'',component:AdminDsoComponent,canActivate:[AdminGuard]},
	{path:'add-user',component:AdminDsoAddComponent,canActivate:[AdminGuard]},
	{path:'update-user/:id',component:AdminDsoUpdateComponent,canActivate:[AdminGuard]},
	//DSO
	{path:'',component:DsoHomePageComponent,canActivate:[DispatcherGuard]},
	{path:'prosumers',component:DsoProsumersPageComponent,canActivate:[DispatcherGuard]},
	{path:'prediction',component:DsoPredictionPageComponent,canActivate:[DispatcherGuard]},
	//PROSUMER
	{path:'',component:ProsumerHomePageComponent,canActivate:[ProsumerGuard]},
	{path:'devices',component:ProsumerDevicesPageComponent,canActivate:[ProsumerGuard]},
	{path:'reports',component:ProsumerReportsPageComponent,canActivate:[ProsumerGuard]},
	//TEST
	// {path:'register',component:RegisterComponent},
	// {path:'change-password',component:ChangePasswordComponent},
	//{path:'reset-password',component:ResetPasswordPageComponent},
	{path:"prosumer-map",component:ProsumersMapComponent},//canActivate:[DispatcherGuard]},
	{path:"map-input",component:MapInputComponent},
	{path:'prosumer-home-page',component:ProsumerHomePageComponent},
	{path:'prosumer-devices-page',component:ProsumerDevicesPageComponent},
	{path:'prosumer-device-page',component:ProsumerDevicePageComponent},
	{path:'today',component:TodayComponent},
	{path:'reset-password',component:ResetPasswordPageComponent},
	{path:"prosumer-map",component:ProsumersMapComponent},//canActivate:[DispatcherGuard]},
	{path:"map-input",component:MapInputComponent},
	{path:"oneprosumertest",component:OneProsumerComponent},
	{path:"producing",component:ProducingConsumingComponent},
	{path:'**',redirectTo:"login"}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
