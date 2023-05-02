import { HttpClientModule } from "@angular/common/http";
import { DecimalPipe } from '@angular/common'; // import DecimalPipe
import { NgModule } from "@angular/core";
import { ReactiveFormsModule, FormsModule } from "@angular/forms";
import { BrowserModule } from "@angular/platform-browser";
import { LeafletModule } from "@asymmetrik/ngx-leaflet";
import { NgxGaugeModule } from "ngx-gauge";
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { AdminDsoAddComponent } from "./components/admin/admin-dso-add/admin-dso-add.component";
import { AdminDsoUpdateComponent } from "./components/admin/admin-dso-update/admin-dso-update.component";
import { AdminDsoComponent } from "./components/admin/admin-dso/admin-dso.component";
import { ChangePasswordComponent } from "./components/change-password/change-password.component";
import { AllProsumersComponent } from "./components/dso/all-prosumers/all-prosumers.component";
import { BarChartComponent } from "./components/dso/charts/bar-chart/bar-chart.component";
import { BarMonthChartComponent } from "./components/dso/charts/bar-month-chart/bar-month-chart.component";
import { BarYearChartComponent } from "./components/dso/charts/bar-year-chart/bar-year-chart.component";
import { LineChartComponent } from "./components/dso/charts/line-chart/line-chart.component";
import { LineDayChartComponent } from "./components/dso/charts/line-day-chart/line-day-chart.component";
import { LineWeekChartComponent } from "./components/dso/charts/line-week-chart/line-week-chart.component";
import { PieChartComponent } from "./components/dso/charts/pie-chart/pie-chart.component";
import { MapsComponent } from "./components/dso/maps/maps.component";
import { MonthConsumptionComponent } from "./components/dso/meter-prosumer/month-consumption/month-consumption.component";
import { MonthProductionComponent } from "./components/dso/meter-prosumer/month-production/month-production.component";
import { TodayConsumptionComponent } from "./components/dso/meter-prosumer/today-consumption/today-consumption.component";
import { ConsumptionMonthWattmeterComponent } from "./components/dso/meter/consumption-month-wattmeter/consumption-month-wattmeter.component";
import { ConsumptionTodayWattmeterComponent } from "./components/dso/meter/consumption-today-wattmeter/consumption-today-wattmeter.component";
import { ConsumptionYearWattmeterComponent } from "./components/dso/meter/consumption-year-wattmeter/consumption-year-wattmeter.component";
import { CurrentPowerWattmeterComponent } from "./components/dso/meter/current-power-wattmeter/current-power-wattmeter.component";
import { NumberOfUsersMeterComponent } from "./components/dso/meter/number-of-users-meter/number-of-users-meter.component";
import { OneProsumerComponent } from "./components/dso/one-prosumer/one-prosumer.component";
import { MapInputComponent } from "./components/map-input/map-input.component";
import { NavbarComponent } from "./components/navbar/navbar.component";
import { ProsumerHouseComponent } from "./components/prosumers/prosumer-meter/prosumer-house/prosumer-house.component";
import { ProsumerSolarComponent } from "./components/prosumers/prosumer-meter/prosumer-solar/prosumer-solar.component";
import { ProsumerTowerComponent } from "./components/prosumers/prosumer-meter/prosumer-tower/prosumer-tower.component";
import { ProsumersMapComponent } from "./components/prosumers-map/prosumers-map.component";
import { SidebarComponent } from "./components/sidebar/sidebar.component";
import { TodayComponent } from "./components/weather/today/today.component";
import { AdminDsoPageComponent } from "./pages/admin/admin-dso-page/admin-dso-page.component";
import { DsoHomePageComponent } from "./pages/dso/dso-home-page/dso-home-page.component";
import { DsoPredictionPageComponent } from "./pages/dso/dso-prediction-page/dso-prediction-page.component";
import { DsoProsumersPageComponent } from "./pages/dso/dso-prosumers-page/dso-prosumers-page.component";
import { ForgotPasswordPageComponent } from "./pages/forgot-password/forgot-password.component";
import { HomePageComponent } from "./pages/home-page/home-page.component";
import { LoginComponent } from "./pages/login/login.component";
import { ProsumerDevicePageComponent } from "./pages/prosumer/prosumer-device-page/prosumer-device-page.component";
import { ProsumerDevicesPageComponent } from "./pages/prosumer/prosumer-devices-page/prosumer-devices-page.component";
import { ProsumerFooterComponent } from "./pages/prosumer/prosumer-footer/prosumer-footer.component";
import { ProsumerHomePageComponent } from "./pages/prosumer/prosumer-home-page/prosumer-home-page.component";
import { ProsumerNavbarComponent } from "./components/prosumer-navbar/prosumer-navbar.component";
import { ProsumerReportsPageComponent } from "./pages/prosumer/prosumer-reports-page/prosumer-reports-page.component";
import { RegisterComponent } from "./pages/register/register.component";
import { ResetPasswordPageComponent } from "./pages/reset-password/reset-password.component";
import { UpdateDeviceComponent } from "./components/prosumers/devices/update-device/update-device.component";
import { AddDeviceComponent } from "./components/prosumers/devices/add-device/add-device.component";
import { AllDevicesComponent } from "./components/prosumers/devices/all-devices/all-devices.component";
import { FutureComponent } from "./components/weather/future/future.component";
import { DsoOneProsumerPageComponent } from "./pages/dso/dso-one-prosumer-page/dso-one-prosumer-page.component";
import { TodayProductionComponent } from "./components/dso/meter-prosumer/today-production/today-production.component";
import { ProsumerAccountPageComponent } from './pages/prosumer/prosumer-account-page/prosumer-account-page.component';
import { ProsumerAccountSettingsPageComponent } from './pages/prosumer/prosumer-account-settings-page/prosumer-account-settings-page.component';
import { HomeDevicesComponent } from './components/prosumers/devices/home-devices/home-devices.component';
import { OneDeviceComponent } from './components/prosumers/devices/one-device/one-device.component';
import { AllDevicesDsoComponent } from './components/dso/all-devices-dso/all-devices-dso.component';
import { PageControlComponent } from './components/page-control/page-control.component';
import { NgxPaginationModule } from "ngx-pagination";
import { FooterComponent } from './components/footer/footer.component';
import { TabelarViewComponent } from './components/dso/tabelar-views/tabelar-view-by-day/tabelar-view.component';
import { TodayDsoComponent } from './components/weather/today-dso/today-dso.component';
import { TabelarViewByMonthComponent } from "./components/dso/tabelar-views/tabelar-view-by-month/tabelar-view-by-month.component";
import { TabelarViewByYearComponent } from './components/dso/tabelar-views/tabelar-view-by-year/tabelar-view-by-year.component';
import { TabelarViewByWeekComponent } from './components/dso/tabelar-views/tabelar-view-by-week/tabelar-view-by-week.component';
import { TodayTabelarProsumerComponent } from './components/dso/tabelar-view-prosumer/today-tabelar-prosumer/today-tabelar-prosumer.component';
import { MonthTabelarProsumerComponent } from './components/dso/tabelar-view-prosumer/month-tabelar-prosumer/month-tabelar-prosumer.component';
import { WeekTabelarProsumerComponent } from './components/dso/tabelar-view-prosumer/week-tabelar-prosumer/week-tabelar-prosumer.component';
import { YearTabelarProsumerComponent } from './components/dso/tabelar-view-prosumer/year-tabelar-prosumer/year-tabelar-prosumer.component';
import { BarMonthProsumerComponent } from './components/dso/charts-prosumer/bar-month-prosumer/bar-month-prosumer.component';
import { BarYearProsumerComponent } from './components/dso/charts-prosumer/bar-year-prosumer/bar-year-prosumer.component';
import { LineDayProsumerComponent } from './components/dso/charts-prosumer/line-day-prosumer/line-day-prosumer.component';
import { LineWeekProsumerComponent } from './components/dso/charts-prosumer/line-week-prosumer/line-week-prosumer.component';
import { PredictionProsumerComponent } from './components/dso/prediction/prediction-prosumer/prediction-prosumer.component';
import { PredictionDeviceComponent } from './components/dso/prediction/prediction-device/prediction-device.component';
import { EmailConfirmationPageComponent } from './pages/email-confirmation-page/email-confirmation-page.component';
import { ChangeEmailConfirmationPageComponent } from './pages/change-email-confirmation-page/change-email-confirmation-page.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatNativeDateModule} from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ProsumerDayGraphComponent } from './components/prosumers/prosumer-graphs/prosumer-day-graph/prosumer-day-graph.component';
import { ProsumerWeekGraphComponent } from './components/prosumers/prosumer-graphs/prosumer-week-graph/prosumer-week-graph.component';
import { ProsumerYearTableComponent } from './components/prosumers/prosumer-tables/prosumer-year-table/prosumer-year-table.component';
import { ProsumerMonthTableComponent } from './components/prosumers/prosumer-tables/prosumer-month-table/prosumer-month-table.component';
import { ProsumerWeekTableComponent } from './components/prosumers/prosumer-tables/prosumer-week-table/prosumer-week-table.component';
import { ProsumerDayTableComponent } from './components/prosumers/prosumer-tables/prosumer-day-table/prosumer-day-table.component';
import { MatFormFieldControl } from "@angular/material/form-field";
import { MatInputModule } from '@angular/material/input';
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
    TodayComponent,
    ProsumersMapComponent,
	  MapInputComponent,
    AdminDsoComponent,
    AdminDsoUpdateComponent,
    AdminDsoAddComponent,
    OneProsumerComponent,
    TodayConsumptionComponent,
    MonthConsumptionComponent,
    MonthProductionComponent,
    AdminDsoComponent,
    ProsumerTowerComponent,
    ProsumerNavbarComponent,
    ProsumerHomePageComponent,
    ProsumerDevicePageComponent,
    ProsumerFooterComponent,
    ProsumerHouseComponent,
    ProsumerSolarComponent,
    ProsumerHomePageComponent,
    HomePageComponent,
    AdminDsoUpdateComponent,
    AdminDsoPageComponent,
    UpdateDeviceComponent,
    AddDeviceComponent,
    AllDevicesComponent,
    FutureComponent,
    DsoOneProsumerPageComponent,
    TodayProductionComponent,
    TodayConsumptionComponent,
    ResetPasswordPageComponent,
    ProsumerAccountPageComponent,
    ProsumerAccountSettingsPageComponent,
    HomeDevicesComponent,
    OneDeviceComponent,
    AllDevicesDsoComponent,
    PageControlComponent,
    FooterComponent,
    TabelarViewComponent,
    TodayDsoComponent,
    TabelarViewByYearComponent,
    TabelarViewByWeekComponent,
    TabelarViewByMonthComponent,
    TodayTabelarProsumerComponent,
    MonthTabelarProsumerComponent,
    WeekTabelarProsumerComponent,
    YearTabelarProsumerComponent,
    BarMonthProsumerComponent,
    BarYearProsumerComponent,
    LineDayProsumerComponent,
    LineWeekProsumerComponent,
    PredictionProsumerComponent,
    PredictionDeviceComponent,
    MonthTabelarProsumerComponent,
    WeekTabelarProsumerComponent,
    YearTabelarProsumerComponent,
    TodayTabelarProsumerComponent,
    EmailConfirmationPageComponent,
    ChangeEmailConfirmationPageComponent,
    ProsumerDayGraphComponent,
    ProsumerWeekGraphComponent,
    ProsumerYearTableComponent,
    ProsumerMonthTableComponent,
    ProsumerWeekTableComponent,
    ProsumerDayTableComponent,
    
  ],
  imports: [
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    NgxGaugeModule,
    LeafletModule,
    FormsModule,
    BrowserModule,
	  NgxPaginationModule,
    MatDatepickerModule,
    BrowserAnimationsModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatInputModule
  ],
  providers: [DecimalPipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
