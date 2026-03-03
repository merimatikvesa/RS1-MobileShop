export interface ProductDto {
  productId: number;
  productName: string;
  model: string;
  price: number;

  brandId: number;
  brandName: string;

  categoryId: number;
  categoryName: string;

  supplierId: number;
  supplierName: string;

  promotionId?: number | null;
  promotionName?: string | null;

  imageUrl?: string | null;
}
export interface PagedResult<T> {
  data: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}
