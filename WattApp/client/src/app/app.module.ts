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
import { AdminProsumersPageComponent } from './pages/admin/admin-prosumers-page/admin-prosumers-page.component';
import { AdminDsoPageComponent } from './pages/admin/admin-dso-page/admin-dso-page.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { NgxGaugeModule } from 'ngx-gauge';
import { LeafletModule } from '@asymmetrik/ngx-leaflet';
import { AllProsumersComponent } from './components/dso/all-prosumers/all-prosumers.component'; 
import { ForgotPasswordPageComponent } from './pages/forgot-password/forgot-password.component';
import { ResetPasswordPageComponent } from './pages/reset-password/reset-password.component';
import { TodayComponent } from './components/today/today.component';
import { AdminDsoComponent } from './components/admin/admin-dso/admin-dso/admin-dso.component';
import { AdminDsoUpdateComponent } from './components/admin/admin-dso/admin-dso-update/admin-dso-update.component';
import { AdminDsoAddComponent } from './components/admin/admin-dso/admin-dso-add/admin-dso-add.component';
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
import { ProsumerTowerComponent } from './components/prosumers/prosumer-meter/prosumer-tower/prosumer-tower.component';
import { ProsumerHouseComponent } from './components/prosumers/prosumer-meter/prosumer-house/prosumer-house.component';
import { ProsumerSolarComponent } from './components/prosumers/prosumer-meter/prosumer-solar/prosumer-solar.component';
import { FutureComponent } from './components/future/future.component';
import { ProsumerOneDevicePageComponent } from './pages/prosumer/prosumer-one-device-page/prosumer-one-device-page.component';
import { DsoOneProsumerPageComponent } from './pages/dso/dso-one-prosumer-page/dso-one-prosumer-page.component';
import { DsoOneProsumerDevicesPageComponent } from './pages/dso/dso-one-prosumer-devices-page/dso-one-prosumer-devices-page.component';
import { AllDevicesComponent } from './components/prosumers/devices/all-devices/all-devices.component';
import { AddDeviceComponent } from './components/prosumers/devices/add-device/add-device.component';
import { UpdateDeviceComponent } from './components/prosumers/devices/update-device/update-device.component';
import { ProsumerDevicesComponent } from './components/dso/prosumer-devices/prosumer-devices.component';

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
    FutureComponent,
    TodayComponent,
    ProsumerTowerComponent,
    ProsumerHouseComponent,
    ProsumerSolarComponent,
    ProsumersMapComponent,
	  MapInputComponent,
    AdminDsoComponent,
    AdminDsoUpdateComponent,
    AdminDsoAddComponent,
    ProsumerOneDevicePageComponent,
    DsoOneProsumerPageComponent,
    DsoOneProsumerDevicesPageComponent,
    AllDevicesComponent,
    AddDeviceComponent,
    UpdateDeviceComponent,
    ProsumerDevicesComponent
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
