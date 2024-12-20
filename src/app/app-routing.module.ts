import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';

import { BillComponent } from './pages/bill/bill.component';
import { SettingsComponent } from './pages/settings/settings.component';
import { AnalyticComponent } from './pages/analytic/analytic.component';
import { MonitoringComponent } from './pages/monitoring/monitoring.component';
import { DemographicComponent } from './pages/demographic/demographic.component';
import { ApplicationComponent } from './pages/application/application.component';
import { DocumentationComponent } from './pages/documentation/documentation.component';
import { LandingPageComponent } from './pages/frontoffice/landing-page/landing-page.component';
import { PopUpLoginComponent } from './pages/frontoffice/pop-up-login/pop-up-login.component';

const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'home', component: HomeComponent },
  { path: 'admin/bills', component: BillComponent },
  { path: 'admin/applications', component: ApplicationComponent },
  { path: 'admin/monitoring', component: MonitoringComponent },
  { path: 'admin/demographics', component: DemographicComponent },
  { path: 'admin/analytics', component: AnalyticComponent },
  { path: 'admin/settings', component: SettingsComponent },
  { path: 'admin/documentation', component: DocumentationComponent },
  { path: 'popup', component: PopUpLoginComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
