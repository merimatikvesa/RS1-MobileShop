export interface ProductCreateDto {
  productName: string;
  model: string;
  price: number;
  brandId: number;
  supplierId: number;
  categoryId: number;
  promotionId?: number | null;
  imagePath?: string | null;
}

export interface ProductUpdateDto extends ProductCreateDto {
  productId: number;
}