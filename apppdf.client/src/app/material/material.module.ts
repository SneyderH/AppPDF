import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,

    //Material Modules
    MatTableModule
  ],

  exports: [
    //Material Modules
    MatTableModule
  ]
})
export class MaterialModule { }
