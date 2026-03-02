import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ShopService {
  private apiUrl = 'https://localhost:7275/api/products';

  constructor(private http: HttpClient) {}

  getProducts(filter: any): Observable<any> {
    let params = new HttpParams()
      .set('pageNumber', filter.pageNumber)
      .set('pageSize', filter.pageSize);

    if (filter.search)
      params = params.set('search', filter.search);

    return this.http.get<any>(this.apiUrl, { params });
  }
}
