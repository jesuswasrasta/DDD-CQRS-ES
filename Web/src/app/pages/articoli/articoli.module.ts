import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { DirectivesModule } from '../../theme/directives/directives.module';

import { ArticoliRoutingModule, RoutedComponents } from './articoli-routing.module';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ArticoliRoutingModule,
    PerfectScrollbarModule,
    DirectivesModule
  ],
  declarations: [ RoutedComponents ]
})

export class ArticoliModule { }
