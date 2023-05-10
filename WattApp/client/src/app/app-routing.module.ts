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
import { AdminGuard } from './guards/admin.guard';
import { ProsumerGuard } from './guards/prosumer.guard';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { DsoPredictionPageComponent } from './pages/dso/dso-prediction-page/dso-prediction-page.component';
import { ProsumerDevicesPageComponent } from './pages/prosumer/prosumer-devices-page/prosumer-devices-page.component';
import { ProsumerReportsPageComponent } from './pages/prosumer/prosumer-reports-page/prosumer-reports-page.component';
import { TodayComponent } from './components/weather/today/today.component';
import { ProsumerDevicePageComponent } from './pages/prosumer/prosumer-device-page/prosumer-device-page.component';
import { UpdateDeviceComponent } from './components/prosumers/devices/update-device/update-device.component';
import { AddDeviceComponent } from './components/prosumers/devices/add-device/add-device.component';
import { FutureComponent } from './components/weather/future/future.component';
import { DsoOneProsumerPageComponent } from './pages/dso/dso-one-prosumer-page/dso-one-prosumer-page.component';
import { ProsumerAccountPageComponent } from './pages/prosumer/prosumer-account-page/prosumer-account-page.component';
import { ProsumerAccountSettingsPageComponent } from './pages/prosumer/prosumer-account-settings-page/prosumer-account-settings-page.component';
import { EmailConfirmationPageComponent } from './pages/email-confirmation-page/email-confirmation-page.component';
import { ChangeEmailConfirmationPageComponent } from './pages/change-email-confirmation-page/change-email-confirmation-page.component';
import { AccountPageComponent } from './pages/account-page/account-page.component';
import { LoaderComponent } from './components/loader/loader.component';
import { ProsumerChangePasswordComponent } from './pages/prosumer/prosumer-change-password/prosumer-change-password.component';
import { PredictionDeviceComponent } from './components/dso/prediction/prediction-device/prediction-device.component';

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
	// {path:'update-user/:id',component:AdminDsoUpdateComponent,canActivate:[AdminGuard]},
	{path:"profile-admin",component:AccountPageComponent,canActivate:[AdminGuard]},
	//DSO
	{path:'',component:DsoHomePageComponent,canActivate:[DispatcherGuard]},
	{path:'prosumers',component:DsoProsumersPageComponent,canActivate:[DispatcherGuard]},
	{path:'prosumer/:id',component:DsoOneProsumerPageComponent,canActivate:[DispatcherGuard]},
	{path:'prediction',component:DsoPredictionPageComponent,canActivate:[DispatcherGuard]},
	{path:"profile-dso",component:AccountPageComponent,canActivate:[DispatcherGuard]},
	//PROSUMER
	{path:'',component:ProsumerHomePageComponent,canActivate:[ProsumerGuard]},
	{path:'devices',component:ProsumerDevicesPageComponent,canActivate:[ProsumerGuard]},
	{path:'device/:id',component:ProsumerDevicePageComponent,canActivate:[ProsumerGuard]},
	{path:'reports',component:ProsumerReportsPageComponent,canActivate:[ProsumerGuard]},
	{path:"device-update/:id",component:UpdateDeviceComponent,canActivate:[ProsumerGuard]},
	{path:"device-add",component:AddDeviceComponent,canActivate:[ProsumerGuard]},
	{path:"profile",component:ProsumerAccountPageComponent,canActivate:[ProsumerGuard]},
	{path:"profile-edit",component:ProsumerAccountSettingsPageComponent,canActivate:[ProsumerGuard]},
	{path:"prosumer-reports",component:ProsumerReportsPageComponent,canActivate:[ProsumerGuard]},
	{path:"loader",component:LoaderComponent},
	{path:"prosumer-change-password",component:ProsumerChangePasswordComponent,canActivate:[ProsumerGuard]},

	
	
	//TEST
	// {path:'register',component:RegisterComponent},
	// {path:'change-password',component:ChangePasswordComponent},
	//{path:'reset-password',component:ResetPasswordPageComponent},
	{path:"prosumer-map",component:ProsumersMapComponent,canActivate:[DispatcherGuard]},
	{path:'prosumer-home-page',component:ProsumerHomePageComponent},
	{path:'prosumer-devices-page',component:ProsumerDevicesPageComponent},
	{path:'prosumer-device-page',component:ProsumerDevicePageComponent},
	{path:'reset-password',component:ResetPasswordPageComponent},
	{path:'prosumer-account-page',component:ProsumerAccountPageComponent},
	{path:'prosumer-account-settings-page',component:ProsumerAccountSettingsPageComponent},
	{path:"prosumer-map",component:ProsumersMapComponent},//canActivate:[DispatcherGuard]},
	//{path:"map-input",component:MapInputComponent},
	{path:"todaytest",component:TodayComponent},
	{path:"futuretest",component:FutureComponent},
	{path:'email-confirmation',component:EmailConfirmationPageComponent},
	{path:"change-email-confirmation", component:ChangeEmailConfirmationPageComponent},
	{path:'**',redirectTo:"login"}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
