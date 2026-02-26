export interface InventoryFilterDto {
  productId?: number;
  minQuantity?: number;
  maxQuantity?: number;
  productName?: string;
  isLowStock?: boolean;
}
