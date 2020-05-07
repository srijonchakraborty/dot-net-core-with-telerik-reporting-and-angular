import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent }  from './app.component';
import { TelerikReportingModule } from '@progress/telerik-angular-report-viewer';

@NgModule({
  imports:      [ BrowserModule, TelerikReportingModule ],
  declarations: [ AppComponent ],
  bootstrap:    [ AppComponent ]
})
export class AppModule { }
