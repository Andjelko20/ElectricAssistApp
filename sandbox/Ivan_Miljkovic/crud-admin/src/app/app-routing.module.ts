import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { UnauthenticatedGuard } from './guards/unauthenticated.guard';
import { AuthenticatedGuard } from './guards/authenticated.guard';
import { RegisterComponent } from './pages/register/register.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { DsoHomePageComponent } from './pages/dso/dso-home-page/dso-home-page.component';
import { DsoProsumersPageComponent } from './pages/dso/dso-prosumers-page/dso-prosumers-page.component';
import { AdminProsumersPageComponent } from './pages/dso/admin/admin-prosumers-page/admin-prosumers-page.component';
import { ForgotPasswordPageComponent } from './pages/forgot-password/forgot-password.component';
import { ResetPasswordPageComponent } from './pages/reset-password/reset-password.component';
import { MapsComponent } from './components/dso/maps/maps.component';
import { AdminDsoComponent } from './components/admin/admin-dso/admin-dso/admin-dso.component';
import { AdminDsoUpdateComponent } from './components/admin/admin-dso/admin-dso-update/admin-dso-update.component';
import { AdminDsoAddComponent } from './components/admin/admin-dso/admin-dso-add/admin-dso-add.component';
 
const routes: Routes = [
	{path:'',redirectTo:'admindso',pathMatch:'full'},
	{path:'login',component:LoginComponent,canActivate:[UnauthenticatedGuard]},
	{path:'admindso',component:AdminDsoComponent,canActivate:[AuthenticatedGuard]},
	{path:'admindsoadd',component:AdminDsoAddComponent,canActivate:[AuthenticatedGuard]},
	{path:'admindsoupdate/:id',component:AdminDsoUpdateComponent,canActivate:[AuthenticatedGuard]},
	{path:'register',component:RegisterComponent},
	{path:'maps',component:MapsComponent},
	{path:'change-password',component:ChangePasswordComponent},
	{path:'dsohome',component:DsoHomePageComponent,canActivate:[AuthenticatedGuard]},
	{path:'dsoprosumer',component:DsoProsumersPageComponent,canActivate:[AuthenticatedGuard]},
	{path:'admin-prosumers-page',component:AdminProsumersPageComponent,canActivate:[AuthenticatedGuard]},
	{path:'forgot-password',component:ForgotPasswordPageComponent,canActivate:[UnauthenticatedGuard]},
	{path:'reset-password/:id',component:ResetPasswordPageComponent,canActivate:[UnauthenticatedGuard]},
	{path:'**',redirectTo:"login"}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }