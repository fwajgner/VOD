import { NgModule } from '@angular/core';
import {
  MatButtonModule,
  MatButtonToggleModule,
  MatIconModule,
  MatMenuModule,
  MatToolbarModule,
  MatGridListModule,
  MatListModule,
  MatCardModule,
} from '@angular/material';
import {CommonModule} from '@angular/common';

const material = [
  MatIconModule,
  MatMenuModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatToolbarModule,
  MatGridListModule,
  MatListModule,
  MatCardModule
];

@NgModule({
  imports: [
    material,
    CommonModule
  ],
  exports: [material]
})
export class MaterialModule { }
