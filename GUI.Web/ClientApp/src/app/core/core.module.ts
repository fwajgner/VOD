import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { CONFIG, Config } from '../model';
import { RouterModule } from '@angular/router';
import { NotFoundComponent } from './not-found/not-found.component';
import {MaterialModule} from '../material/material.module';

const config: Config = {
  apiUrl: 'https://localhost:44350/api'
};

@NgModule({
  imports: [
    RouterModule,
    CommonModule,
    MaterialModule
  ],
  providers: [
    { provide: CONFIG, useValue: config }
  ],
  declarations: [
    NavMenuComponent,
    NotFoundComponent
  ],
  exports: [
    NavMenuComponent
  ]
})
export class CoreModule { }
