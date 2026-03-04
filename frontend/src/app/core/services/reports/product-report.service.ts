import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ReportsService {
  constructor(private http: HttpClient) {}

  downloadProductsReport(params: {
    id?: number | null;
    productName?: string | null;
    startDate: Date;
    endDate: Date;
  }): Observable<Blob> {
    let httpParams = new HttpParams()
      .set('startDate', this.toYmd(params.startDate))
      .set('endDate', this.toYmd(params.endDate));

    if (params.id && params.id > 0) {
      httpParams = httpParams.set('id', params.id);
    }

    if (params.productName && params.productName.trim().length > 0) {
      httpParams = httpParams.set('productName', params.productName.trim());
    }

    const API = 'https://localhost:7275'; 

      return this.http.get(`${API}/api/reports/products`, {
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