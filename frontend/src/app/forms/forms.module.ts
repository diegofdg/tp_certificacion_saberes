import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AppRoutingModule } from '../app-routing.module';
import { ResetpasswordComponent } from './resetpassword/resetpassword.component';
import { NavUserComponent } from './nav-user/nav-user.component';
import { SectionsModule } from './sections/sections.module';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule as FormsModuleAngular } from '@angular/forms';
import { OperationPanelComponent } from './operation-panel/operation-panel.component';

@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    DashboardComponent,
    ResetpasswordComponent,
    NavUserComponent,
    OperationPanelComponent
  ],
  imports: [
    CommonModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModuleAngular,
    SectionsModule
  ],
  exports:[
    LoginComponent,
    RegisterComponent,
    DashboardComponent,
    ReactiveFormsModule,
    FormsModuleAngular,
    OperationPanelComponent
  ]
})
export class FormsModule { }
