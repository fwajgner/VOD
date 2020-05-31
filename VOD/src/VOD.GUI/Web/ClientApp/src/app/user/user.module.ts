import { NgModule } from '@angular/core';
import { LogInComponent } from './log-in/log-in.component';
import {SharedModule} from '../shared/shared.module';
import {RouterModule, Routes} from '@angular/router';
import {ReactiveFormsModule} from '@angular/forms';

const routes: Routes = [
  { path: '', component: LogInComponent}
];

@NgModule({
  declarations: [
    LogInComponent
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
