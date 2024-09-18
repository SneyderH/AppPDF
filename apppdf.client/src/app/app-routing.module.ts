import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PDFComponent } from './filepdf/filepdf.component';

const routes: Routes = [
  { path: '', component: PDFComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
