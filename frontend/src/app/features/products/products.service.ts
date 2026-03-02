import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PagedResult, ProductDto} from './product.dto';
import { BrandDto} from './brand.dto';
import { CategoryDto} from './category.dto';
import { ProductCreateDto, ProductUpdateDto } from './product-create-update.dto';
import { PromotionDto } from './promotion.dto';
import { SupplierDto } from './supplier.dto';

export interface ProductFilter {
  search?: string | null;
  categoryId?: number | null;
  brandId?: number | null;
  minPrice?: number | null;
  maxPrice?: number | null;
  pageNumber: number;
  pageSize: number;
}

@Injectable({ providedIn: 'root' })
export class ProductsService {
  private apiUrl = 'https://localhost:7275/api/products';

  constructor(private http: HttpClient) {}

  getProducts(filter: ProductFilter) {
    let params = new HttpParams();
    Object.entries(filter).forEach(([key, value]) => {
      if (value !== null && value !== undefined && value !== '') {
        params = params.set(key, value.toString());
      }
    });

    return this.http.get<PagedResult<ProductDto>>(this.apiUrl, { params });
  }
  update(dto: ProductUpdateDto) {
  return this.http.put(this.apiUrl, dto);
  } 
  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
  getBrands() {
  return this.http.get<BrandDto[]>('https://localhost:7275/api/brands');
  } 

  getCategories() {
  return this.http.get<CategoryDto[]>('https://localhost:7275/api/categories');
  }
  getPromotions(){
    return this.http.get<PromotionDto[]>('https://localhost:7275/api/promotions')
  }
   getSuppliers(){
    return this.http.get<SupplierDto[]>('https://localhost:7275/api/suppliers')
  }
  create(dto: ProductCreateDto) {
  return this.http.post(this.apiUrl, dto);
  }
  uploadImages(productId: number, files: File[]) {
  const formData = new FormData();
  files.forEach(f => formData.append('files', f));
  return this.http.post(`${this.apiUrl}/${productId}/images`, formData);
  }
}