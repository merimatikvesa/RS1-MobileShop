import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PagedResult, ProductDto} from './product.dto';
import { HttpEvent } from '@angular/common/http';
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
export interface ProductImageDto {
  productImageId: number;
  imageId: number;
  imagePath: string;
}

@Injectable({ providedIn: 'root' })
export class ProductsService {
  private apiUrl = 'https://rs1-mobileshop-api.onrender.com/api/products';

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
  return this.http.get<BrandDto[]>('https://rs1-mobileshop-api.onrender.com/api/brands');
  } 

  getCategories() {
  return this.http.get<CategoryDto[]>('https://rs1-mobileshop-api.onrender.com/api/categories');
  }
  getPromotions(){
    return this.http.get<PromotionDto[]>('https://rs1-mobileshop-api.onrender.com/api/promotions')
  }
   getSuppliers(){
    return this.http.get<SupplierDto[]>('https://rs1-mobileshop-api.onrender.com/api/suppliers')
  }
  create(dto: ProductCreateDto) {
  return this.http.post(this.apiUrl, dto);
  }
  uploadImages(productId: number, files: File[]) {
  const formData = new FormData();
  files.forEach(f => formData.append('files', f));

  return this.http.post<any>(
    `${this.apiUrl}/${productId}/images`,
    formData,
    { observe: 'events', reportProgress: true }
  );
  }
  getProductImages(productId: number) {
  return this.http.get<any[]>(`${this.apiUrl}/${productId}/images`);
  }
  deleteProductImage(productId: number, productImageId: number) {
  return this.http.delete(`${this.apiUrl}/${productId}/images/${productImageId}`);
  }
}