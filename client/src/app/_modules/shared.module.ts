import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { NgxSpinnerModule } from "ngx-spinner";
import { PaginationModule } from 'ngx-bootstrap/pagination';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(), // forRoot means it has some services/components that need to initialize alongside the root module
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
    NgxGalleryModule,
    NgxSpinnerModule,
    PaginationModule.forRoot()
  ],
  exports: [
    BsDropdownModule,
    ToastrModule,
    NgxGalleryModule,
    NgxSpinnerModule,
    PaginationModule
  ]
})
export class SharedModule { }
