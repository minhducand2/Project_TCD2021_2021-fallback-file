import { Directive, ElementRef, HostListener, Input } from '@angular/core';
import { ExportService } from './export-excel.service';

@Directive({
  selector: '[appExport]'
})
export class ExportDirective {

  constructor(private exportService: ExportService) { }

  @Input('appExport') Company: any[];
  @Input() fileName: string;

  @HostListener('click', ['$event']) onClick($event) {
    console.log('clicked: ' + $event);
    this.exportService.exportExcel(this.Company, this.fileName);
  }

}