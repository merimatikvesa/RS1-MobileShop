import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { InventoryDto } from './inventory.dto';
import { InventoryFilterDto } from './inventory-filter.dto';
import {InventoryUpsertDto} from './inventory-create-update.dto';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {

  private apiUrl = 'https://localhost:7275/api/inventory'; // port

  constructor(private http: HttpClient) {}

  getInventory(filter: InventoryFilterDto) {
    let params = new HttpParams();

    Object.entries(filter).forEach(([key, value]) => {
      if (value !== null && value !== undefined && value !== '') {
        params = params.set(key, value.toString());
      }
    });

    return this.http.get<InventoryDto[]>(this.apiUrl, { params });
  }
  createInventory(dto: InventoryUpsertDto) {
    return this.http.post(this.apiUrl, dto);
  }

  updateInventory(productId: number, dto: any) {
    return this.http.put(
      `${this.apiUrl}/${productId}`,
      dto
    );
  }
  deleteInventory(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
