import { Directive, ElementRef, HostListener, Input } from '@angular/core';
import { ExportService } from './export-excel.service';

@Directive({
  selector: '[appExport]'
})
export class ExportDirectiveCandidate {

  constructor(private exportService: ExportService) { }

  @Input('appExport') Candidate: any[];
  @Input() fileName: string;

  @HostListener('click', ['$event']) onClick($event) {
    console.log('clicked: ' + $event);
    this.exportService.exportExcel(this.Candidate, this.fileName);
  }

}