import { Component } from '@angular/core';

@Component({
  selector: 'my-app',
  template: `<tr-viewer #viewer1 
    [containerStyle]="viewerContainerStyle"
    [serviceUrl]="'https://demos.telerik.com/reporting/api/reports/'"
    [reportSource]="{
        report: 'Telerik.Reporting.Examples.CSharp.ReportCatalog, CSharp.ReportLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null',
        parameters: {}
    }"
    [viewMode]="'INTERACTIVE'"
    [scaleMode]="'SPECIFIC'"
    [scale]="1.0"
    [ready]="ready"
    [viewerToolTipOpening]="viewerToolTipOpening"
    [enableAccessibility]="false">
</tr-viewer>
<button (click)="viewer1.refreshReport()">Refresh</button>
<button (click)="viewer1.commands.print.exec()">Print</button>`
})
export class AppComponent {
  title = "Report Viewer";
  viewerContainerStyle = {
    position: 'relative',
    width: '1000px',
    height: '800px',
    ['font-family']: 'ms sans serif'
  };

  ready() { console.log('ready'); }
  viewerToolTipOpening(e: any, args: any) { console.log('viewerToolTipOpening ' + args.toolTip.text); }
}
