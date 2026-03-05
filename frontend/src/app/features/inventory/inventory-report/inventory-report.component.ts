import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { InventoryReportService } from '../../../core/services/reports/inventory-report.service';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  selector: 'app-inventory-report',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  templateUrl: './inventory-report.component.html',
  styleUrls: ['./inventory-report.component.css']
})

export class InventoryReportComponent {
  reportForm: any;
  constructor(
    private fb: FormBuilder,
    private reportService: InventoryReportService
  ) {
    this.reportForm = this.fb.group({
      productId: [null],
      startDate: [null],
      endDate: [null]
    });
  }

  generatePdf() {

    const v = this.reportForm.value;

    this.reportService.downloadInventoryReport({
      productId: v.productId,
      startDate: v.startDate!,
      endDate: v.endDate!
    }).subscribe({
      next: (blob) => {
        this.downloadBlob(blob, `inventory_report_${Date.now()}.pdf`);
      },
      error: (err) => {
        console.error(err);
      }
    });

  }

  private downloadBlob(blob: Blob, filename: string) {
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    a.click();
    window.URL.revokeObjectURL(url);
  }

}
