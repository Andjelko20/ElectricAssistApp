import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './pages/login/login.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { RegisterComponent } from './pages/register/register.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';


import { ProsumerHomePageComponent } from './pages/prosumer/prosumer-home-page/prosumer-home-page.component';
import { ProsumerDevicesPageComponent } from './pages/prosumer/prosumer-devices-page/prosumer-devices-page.component';
import { ProsumerReportsPageComponent } from './pages/prosumer/prosumer-reports-page/prosumer-reports-page.component';
import { DsoHomePageComponent } from './pages/dso/dso-home-page/dso-home-page.component';
import { DsoPredictionPageComponent } from './pages/dso/dso-prediction-page/dso-prediction-page.component';
import { DsoProsumersPageComponent } from './pages/dso/dso-prosumers-page/dso-prosumers-page.component';
import { AdminProsumersPageComponent } from './pages/dso/admin/admin-prosumers-page/admin-prosumers-page.component';
import { AdminDsoPageComponent } from './pages/dso/admin/admin-dso-page/admin-dso-page.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { NgxGaugeModule } from 'ngx-gauge';
import { LeafletModule } from '@asymmetrik/ngx-leaflet';
import { AllProsumersComponent } from './components/dso/all-prosumers/all-prosumers.component'; 
import { ForgotPasswordPageComponent } from './pages/forgot-password/forgot-password.component';
import { ResetPasswordPageComponent } from './pages/reset-password/reset-password.component';
import { TodayComponent } from './components/today/today.component';

import { AdminDsoUpdateComponent } from './components/admin/admin-dso-update/admin-dso-update.component';
import { AdminDsoAddComponent } from './components/admin/admin-dso-add/admin-dso-add.component';
import { PieChartComponent } from './components/dso/charts/pie-chart/pie-chart.component';
import { LineChartComponent } from './components/dso/charts/line-chart/line-chart.component';
import { BarChartComponent } from './components/dso/charts/bar-chart/bar-chart.component';
import { BarMonthChartComponent } from './components/dso/charts/bar-month-chart/bar-month-chart.component';
import { BarYearChartComponent } from './components/dso/charts/bar-year-chart/bar-year-chart.component';
import { LineDayChartComponent } from './components/dso/charts/line-day-chart/line-day-chart.component';
import { LineWeekChartComponent } from './components/dso/charts/line-week-chart/line-week-chart.component';
import { MapsComponent } from './components/dso/maps/maps.component';
import { ConsumptionMonthWattmeterComponent } from './components/dso/meter/consumption-month-wattmeter/consumption-month-wattmeter.component';
import { ConsumptionTodayWattmeterComponent } from './components/dso/meter/consumption-today-wattmeter/consumption-today-wattmeter.component';
import { ConsumptionYearWattmeterComponent } from './components/dso/meter/consumption-year-wattmeter/consumption-year-wattmeter.component';
import { CurrentPowerWattmeterComponent } from './components/dso/meter/current-power-wattmeter/current-power-wattmeter.component';
import { NumberOfUsersMeterComponent } from './components/dso/meter/number-of-users-meter/number-of-users-meter.component';
import { ProsumersMapComponent } from './components/prosumers-map/prosumers-map.component';
import { MapInputComponent } from './components/map-input/map-input.component';
import { FutureComponent } from './components/future/future.component';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { ProsumerNavbarComponent } from './pages/prosumer/prosumer-navbar/prosumer-navbar.component';
import { ProsumerFooterComponent } from './pages/prosumer/prosumer-footer/prosumer-footer.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    ChangePasswordComponent,
    PieChartComponent,
    LineChartComponent,
    BarChartComponent,
    ProsumerHomePageComponent,
    ProsumerDevicesPageComponent,
    ProsumerReportsPageComponent,
    DsoHomePageComponent,
    DsoPredictionPageComponent,
    DsoProsumersPageComponent,
    AdminProsumersPageComponent,
    AdminDsoPageComponent,
    NavbarComponent,
    SidebarComponent,
    CurrentPowerWattmeterComponent,
    ConsumptionTodayWattmeterComponent,
    ConsumptionMonthWattmeterComponent,
    ConsumptionYearWattmeterComponent,
    NumberOfUsersMeterComponent,
    MapsComponent,
    AllProsumersComponent,
    LineDayChartComponent,
    BarYearChartComponent,
    BarMonthChartComponent,
    LineWeekChartComponent,
    ForgotPasswordPageComponent,
    ResetPasswordPageComponent,
    TodayComponent,
    ProsumersMapComponent,
	  MapInputComponent,
    AdminDsoUpdateComponent,
    AdminDsoAddComponent,
    HomePageComponent,
    ProsumerNavbarComponent,
    ProsumerFooterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    NgxGaugeModule,
    LeafletModule,
    FormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
