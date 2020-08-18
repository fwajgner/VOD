import { NgModule } from '@angular/core';
import { LogInComponent } from './log-in/log-in.component';
import {SharedModule} from '../shared/shared.module';
import {RouterModule, Routes} from '@angular/router';
import {ReactiveFormsModule} from '@angular/forms';
import { RegisterComponent } from './register/register.component';

const routes: Routes = [
  { path: 'login', component: LogInComponent},
  { path: 'register', component: RegisterComponent}
];

@NgModule({
  declarations: [
    LogInComponent,
    RegisterComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule
  ],
  exports: [
    LogInComponent
  ]
})
export class UserModule { }
