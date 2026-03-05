import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { HttpClient} from '@angular/common/http';
import { PromotionDto } from '../promotion.dto';
import { SupplierDto } from '../supplier.dto';
import { BrandDto } from '../brand.dto';
import { CategoryDto } from '../category.dto';
import { ProductCreateDto } from '../product-create-update.dto';
import { ProductDto } from '../product.dto';
import { ProductsService } from '../products.service';

export interface ProductCreateDialogData {
  brands: BrandDto[];
  categories: CategoryDto[];
  suppliers: SupplierDto[];
  promotions: PromotionDto[];
  product?: ProductDto;
  existingImages?: any;
}

@Component({
  selector: 'app-product-create-dialog',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatCheckboxModule
  ],
  templateUrl: './product-create-dialog.component.html',
  styleUrl: './product-create-dialog.component.css'
})
export class ProductCreateDialogComponent {
  form: FormGroup;
 // selectedFiles: File[] = [];
  selectedFiles: File[] = [];
  previewUrls: string[] = [];
 
  uploading = false;
  uploadProgress = 0;
  uploadError: string | null = null;

// promijeni ako ti je drugačiji base URL
private apiBaseUrl = 'https://localhost:7275';
  
  constructor(
    private fb: FormBuilder,
    private productsService: ProductsService,
    private dialogRef: MatDialogRef<ProductCreateDialogComponent>,
    private http: HttpClient,
    @Inject(MAT_DIALOG_DATA) public data: ProductCreateDialogData
  ) {
    this.selectedFiles = [];
    this.previewUrls = [];

    this.form = this.fb.group({
    productName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
    model: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(100)]],
    price: [0, [Validators.required, Validators.min(0)]],
    brandId: [null as number | null, [Validators.required]],
    supplierId: [null as number | null, [Validators.required]],
    categoryId: [null as number | null, [Validators.required]],
    promotionId: [null as number | null],
    imagePath: [''],

    // UI-only
    isPhone: [false],
  });
  if (this.data?.product) {
  const p = this.data.product;
  this.form.patchValue({
    productName: p.productName,
    model: p.model,
    price: p.price,
    brandId: p.brandId,
    supplierId: p.supplierId,
    categoryId: p.categoryId,
    promotionId: p.promotionId ?? null
  });
}
  }

  onFileSelected(event: Event) {
  const input = event.target as HTMLInputElement;
  if (!input.files?.length) return;

  const newFiles = Array.from(input.files);

  const total = this.selectedFiles.length + newFiles.length;

  if (total > 5) {
    alert('Maximum 5 images allowed.');
  }

  const allowed = newFiles.slice(0, 5 - this.selectedFiles.length);

  this.selectedFiles.push(...allowed);

  for (const file of allowed) {
    const reader = new FileReader();
    reader.onload = () => this.previewUrls.push(reader.result as string);
    reader.readAsDataURL(file);
  }

  input.value = '';
  }

  
removeImage(index: number) {
  this.selectedFiles.splice(index, 1);
  this.previewUrls.splice(index, 1);
}
 clearImages() {
  this.selectedFiles = [];
  this.previewUrls = [];
 }

  cancel() {
    this.dialogRef.close();
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const v = this.form.getRawValue();

    const dto: ProductCreateDto = {
      productName: v.productName!.trim(),
      model: v.model!.trim(),
      price: Number(v.price),
      brandId: v.brandId!,
      supplierId: v.supplierId!,
      categoryId: v.categoryId!,
      promotionId: v.promotionId ?? null,
      imagePath: v.imagePath ?? null
    };


    this.dialogRef.close({ dto, isPhone: !!v.isPhone, files: this.selectedFiles ?? []  });
  }
  removeExistingImage(img: any) {
  const productImageId = img.productImageId ?? img.ProductImageId;
  const productId = this.data.product?.productId ?? this.data.product?.productId;

  if (!productImageId || !productId) return;

  // optimistic UI: ukloni iz liste
  this.data.existingImages = (this.data.existingImages ?? []).filter((x:any) => {
    const id = x.productImageId ?? x.ProductImageId;
    return id !== productImageId;
  });

  // pozovi API
  this.productsService.deleteProductImage(productId, productImageId).subscribe({
    next: () => {},
    error: () => {
      // ako fail-a, možeš samo reload (ili vrati nazad)
      this.productsService.getProductImages(productId).subscribe(images => {
        this.data.existingImages = images;
      });
    }
  });
}

}
