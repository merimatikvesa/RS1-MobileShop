import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ProductCreateDialogComponent } from './product-create-dialog/product-create-dialog.component';
import { AuthService } from '../../core/services/auth/auth.service';
import { Router } from '@angular/router';
import { ProductsService } from './products.service';
import { ProductDto } from './product.dto';
import { BrandDto } from './brand.dto';
import { CategoryDto } from './category.dto';
import { SupplierDto } from './supplier.dto';
import { PromotionDto } from './promotion.dto';
import { RouterModule } from '@angular/router';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { ReportsService } from '../../core/services/reports/product-report.service';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { HttpEventType } from '@angular/common/http';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatSnackBarModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressBarModule,
    MatTableModule,
    MatPaginatorModule,
    MatSelectModule,
    MatDialogModule,
    RouterModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  products: ProductDto[] = [];
  brands: BrandDto[] = [];
  categories: CategoryDto[] = [];
  suppliers: SupplierDto[] = [];
  promotions: PromotionDto[] = [];
  filterForm!: FormGroup;
  private destroy$ = new Subject<void>();
  reportForm!: FormGroup;
  loading = false;

  totalCount = 0;
  pageNumber = 1;
  pageSize = 10;
  uploadingImages = false;
  uploadProgress = 0;

  displayedColumns: string[] = [
    'productName',
    'model',
    'price',
    'brandName',
    'supplierName',
    'categoryName',
    'promotionName',
    'actions'
  ];

  constructor(
    private fb: FormBuilder,
    private productsService: ProductsService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    public auth: AuthService,
    private router: Router,
    private reports: ReportsService
  ) {}

  ngOnInit(): void {
    this.filterForm = this.fb.group({
      search: ['', [Validators.maxLength(100)]],
      categoryId: [null],
      brandId: [null],
      minPrice: [null, Validators.min(0)],
      maxPrice: [null, Validators.min(0)]
    });

    this.reportForm = this.fb.group({
      id: [null],
      productName: [''],
      startDate: [new Date(), Validators.required],
     endDate: [new Date(), Validators.required]
    });

    // LIVE FILTER (debounce)
  this.filterForm.valueChanges
  .pipe(
    debounceTime(350),
    distinctUntilChanged((a, b) => JSON.stringify(a) === JSON.stringify(b)),
    takeUntil(this.destroy$)
  )
  .subscribe(() => {
    const search = (this.filterForm.value.search ?? '').trim();

    
    if (search.length === 1) return;

 
    this.pageNumber = 1;
    this.loadProducts();
  });

    this.loadProducts();
    this.productsService.getBrands().subscribe(b => this.brands = b);
    this.productsService.getCategories().subscribe(c => this.categories = c);
    this.productsService.getSuppliers().subscribe(s=> this.suppliers = s);
    this.productsService.getPromotions().subscribe(p=> this.promotions = p);
    


  }

  private showMessage(message: string, isError = false) {
    this.snackBar.open(message, 'OK', {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
      panelClass: isError ? 'error-snackbar' : 'success-snackbar'
    });
  }

  loadProducts() {
   
    const { minPrice, maxPrice, search } = this.filterForm.value;

    if (search && search.trim().length === 1) { 
      return;
    }

    if (minPrice != null && maxPrice != null && minPrice > maxPrice) {
      this.showMessage('MinPrice cannot be greater than MaxPrice.', true);
      return;
    }

    this.loading = true;

    this.productsService.getProducts({
      ...this.filterForm.value,
      search: search?.trim() ?? '',
      pageNumber: this.pageNumber,
      pageSize: this.pageSize
    }).subscribe({
      next: (result) => {
        this.products = result.data;
        this.totalCount = result.totalCount;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.showMessage('Error loading products.', true);
        this.loading = false;
      }
    });
  }

  

  clearFilter() {
    this.filterForm.reset({
      search: '',
      categoryId: null,
      brandId: null,
      minPrice: null,
      maxPrice: null
    });
    this.pageNumber = 1;
    this.loadProducts();
  }

  onPageChange(event: any) {
    this.pageNumber = event.pageIndex + 1; 
    this.pageSize = event.pageSize;
    this.loadProducts();
  }


  addNew() {
  const dialogRef = this.dialog.open(ProductCreateDialogComponent, {
    width: '520px',
    disableClose: true,
    data: {
      brands: this.brands ?? [],
      categories: this.categories ?? [],
      suppliers: this.suppliers ?? [],
      promotions: this.promotions ?? []
    }
  });

  dialogRef.afterClosed().subscribe(result => {
    if (!result) return;

    const { dto, files, isPhone } = result;

    this.loading = true;
    //1. create product
    this.productsService.create(dto).subscribe({
      next: (created: any) => {
        const newId = created?.productId;

        
        if (!newId) {
          this.showMessage('Product created, but productId was not returned.', true);
          this.loading = false;
          this.loadProducts();
          return;
        }

      
        if (!files || files.length === 0) {
          this.showMessage('Product created successfully');
          this.loading = false;
          this.loadProducts();
          return;
        }
        this.uploadingImages = true;
        this.uploadProgress=0;
     
        this.productsService.uploadImages(newId, files).subscribe({
          next:(event: any) => {
            // Upload progress
            if (event.type === HttpEventType.UploadProgress) {
            const total = event.total ?? 1;
            this.uploadProgress = Math.round((100 * event.loaded) / total);
            }

            // Response finished
            if (event.type === HttpEventType.Response) {
            this.uploadingImages = false;
            this.uploadProgress = 0;
            this.showMessage('Product created with images');
            this.loading = false;
            this.loadProducts();
            }
          },
          error: (err: any) => {
          this.uploadingImages = false;
          const msg = err?.error?.error || 'Product created, but image upload failed';
          this.showMessage(msg, true);
          this.loading = false;
          this.loadProducts();
        }
        });
      },
      error: (err: any) => {
        const msg = err?.error?.error || 'Error creating product';
        this.showMessage(msg, true);
        this.loading = false;
      }
    });
  });
 }

 
 edit(item: any) {
  
  const openEditDialog = (existingImages: any[] = []) => {
    const dialogRef = this.dialog.open(ProductCreateDialogComponent, {
      width: '520px',
      disableClose: true,
      data: {
        brands: this.brands ?? [],
        categories: this.categories ?? [],
        suppliers: this.suppliers ?? [],
        promotions: this.promotions ?? [],
        product: item,
        existingImages
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (!result) return;

      const { dto, files } = result;
      this.loading = true;

      const updateDto = {
        productId: item.productId,
        productName: dto.productName,
        model: dto.model,
        price: dto.price,
        brandId: dto.brandId,
        supplierId: dto.supplierId,
        categoryId: dto.categoryId,
        promotionId: dto.promotionId ?? null
      };

      this.productsService.update(updateDto).subscribe({
        next: () => {
          if (!files || files.length === 0) {
            this.showMessage('Product updated successfully');
            this.loading = false;
            this.loadProducts();
            return;
          }

          this.uploadingImages = true;
          this.uploadProgress = 0;

          this.productsService.uploadImages(item.productId, files).subscribe({
            next: (event: any) => {
              if (event.type === 3) {
                const total = event.total ?? 1;
                this.uploadProgress = Math.round((100 * event.loaded) / total);
              }
              if (event.type === 4) {
                this.uploadingImages = false;
                this.showMessage('Product updated with images');
                this.loading = false;
                this.loadProducts();
              }
            },
            error: (err: any) => {
              this.uploadingImages = false;
              const msg = err?.error?.error || 'Product updated, but image upload failed';
              this.showMessage(msg, true);
              this.loading = false;
              this.loadProducts();
            }
          });
        },
        error: (err: any) => {
          this.loading = false;
          const msg = err?.error?.error || 'Error updating product';
          this.showMessage(msg, true);
        }
      });
    });
  };

 
  this.productsService.getProductImages(item.productId).subscribe({
    next: (images) => openEditDialog(images),
    error: () => openEditDialog([]) 
  });
  }

  delete(item: ProductDto) {
    if (!confirm(`Delete product: ${item.productName} ${item.model}?`)) return;

    this.productsService.delete(item.productId).subscribe({
      next: () => {
        this.showMessage('Product deleted successfully.');
     
        if (this.products.length === 1 && this.pageNumber > 1) this.pageNumber--;
        this.loadProducts();
      },
      error: (err) => {
        console.error(err);
        const msg = err?.error?.error || 'Error deleting product.';
        this.showMessage(msg, true);
      }
    });
  }
  logout() {
    this.auth.logout();
    this.router.navigate(['/']);
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  generatePdf() {
  if (this.reportForm.invalid) {
    this.showMessage('Please select Start date and End date.', true);
    return;
  }

  const v = this.reportForm.value;

  this.loading = true;

  this.reports.downloadProductsReport({
    id: v.id,
    productName: v.productName,
    startDate: v.startDate,
    endDate: v.endDate
  }).subscribe({
    next: (blob) => {
      console.log('Blob type:', blob.type, 'size:', blob.size);
      this.downloadBlob(blob, `products_report_${Date.now()}.pdf`);
      this.loading = false;
      this.showMessage('PDF report generated.');
    },
    error: (err) => {
      console.error(err);
      this.loading = false;
      this.showMessage('Error generating PDF report.', true);
    }
  });
  }   

  private downloadBlob(blob: Blob, filename: string) {
  const url = window.URL.createObjectURL(blob);
  const a = document.createElement('a');
  a.href = url;
  a.download = filename;
  a.click();
  window.URL.revokeObjectURL(url);
  }


}
