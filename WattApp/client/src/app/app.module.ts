import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './pages/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { UpdateComponent } from './components/update/update.component';
import { AddCompComponent } from './components/add-comp/add-comp.component';
import { RegisterComponent } from './pages/register/register.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { PieChartComponent } from './components/charts/pie-chart/pie-chart.component';
import { LineChartComponent } from './components/charts/line-chart/line-chart.component';
import { BarChartComponent } from './components/charts/bar-chart/bar-chart.component';

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
import { CurrentPowerWattmeterComponent } from './components/meter/current-power-wattmeter/current-power-wattmeter.component';

import { NgxGaugeModule } from 'ngx-gauge';
import { NumberOfUsersMeterComponent } from './components/meter/number-of-users-meter/number-of-users-meter.component';
import { MapsComponent } from './components/maps/maps.component';
import { LeafletModule } from '@asymmetrik/ngx-leaflet';
import { AllProsumersComponent } from './components/all-prosumers/all-prosumers.component'; 
import { ConsumptionTodayWattmeterComponent } from './components/meter/consumption-today-wattmeter/consumption-today-wattmeter.component';
import { ConsumptionYearWattmeterComponent } from './components/meter/consumption-year-wattmeter/consumption-year-wattmeter.component';
import { ConsumptionMonthWattmeterComponent } from './components/meter/consumption-month-wattmeter/consumption-month-wattmeter.component';
import { LineDayChartComponent } from './components/charts/line-day-chart/line-day-chart.component';
import { LineWeekChartComponent } from './components/charts/line-week-chart/line-week-chart.component';
import { BarMonthChartComponent } from './components/charts/bar-month-chart/bar-month-chart.component';
import { BarYearChartComponent } from './components/charts/bar-year-chart/bar-year-chart.component';
import { ForgotPasswordPageComponent } from './pages/forgot-password/forgot-password.component';
import { ResetPasswordPageComponent } from './pages/reset-password/reset-password.component';
import { TodayComponent } from './components/today/today.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    UpdateComponent,
    AddCompComponent,
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
    TodayComponent

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
