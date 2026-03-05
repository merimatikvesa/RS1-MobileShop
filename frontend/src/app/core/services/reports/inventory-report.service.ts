import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class InventoryReportService {

  constructor(private http: HttpClient) {}

  downloadInventoryReport(params: {
    productId?: number | null;
    startDate: Date;
    endDate: Date;
  }): Observable<Blob> {

    let httpParams = new HttpParams()
      .set('startDate', this.toYmd(params.startDate))
      .set('endDate', this.toYmd(params.endDate));

    if (params.productId && params.productId > 0) {
      httpParams = httpParams.set('productId', params.productId);
    }

    const API = 'https://localhost:7275';

    return this.http.get(`${API}/api/reports/inventory`, {
      params: httpParams,
      responseType: 'blob'
    });
  }

  private toYmd(d: Date): string {
    const yyyy = d.getFullYear();
    const mm = String(d.getMonth() + 1).padStart(2, '0');
    const dd = String(d.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }
}
