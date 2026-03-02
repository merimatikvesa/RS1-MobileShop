import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { InventoryService } from './inventory.service';
import { InventoryDto } from './inventory.dto';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatStepperModule } from '@angular/material/stepper';
import { ViewChild } from '@angular/core';
import { MatStepper } from '@angular/material/stepper';
import {AuthService} from '../../core/services/auth/auth.service';
import {Router, RouterModule} from '@angular/router';

@Component({
  selector: 'app-inventory',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatButtonModule,
    MatProgressBarModule,
    MatTableModule,
    MatPaginatorModule,
    MatStepperModule,
    RouterModule
  ],
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css']
})
export class InventoryComponent implements OnInit {

  inventories: InventoryDto[] = [];
  //two forms
  filterForm!: FormGroup;
 // inventoryForm!: FormGroup;

  productStepForm!: FormGroup;
  inventoryStepForm!: FormGroup;

  @ViewChild(MatStepper) stepper!: MatStepper;

  editingId: number | null = null;

  loading = false;

  totalCount=0;
  pageNumber=1;
  pageSize=10;

  showMessage(message: string, isError = false) {
    this.snackBar.open(message, 'OK', {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
      panelClass: isError ? 'error-snackbar' : 'success-snackbar'
    });
  }

  constructor(
    private fb: FormBuilder,
    private inventoryService: InventoryService,
    private snackBar: MatSnackBar,
    public auth: AuthService,
    private router: Router,
  ) {}

  ngOnInit(): void {

    // filter
    this.filterForm = this.fb.group({
      productId: [null],
      minQuantity: [null, Validators.min(0)],
      maxQuantity: [null, Validators.min(0)],
      productName: [''],
      isLowStock: [false]
    });

    // create/update
 /*   this.inventoryForm = this.fb.group({
      productId: [null, Validators.required],
      quantity: [null, [Validators.required, Validators.min(0)]]
    }); */

    this.productStepForm = this.fb.group({
      productId: [null, Validators.required]
    });

    this.inventoryStepForm = this.fb.group({
      quantity: [null, [Validators.required, Validators.min(0)]]
    });

    this.loadInventory();
  }
// read
  loadInventory() {
    this.inventoryService.getInventory({
      ...this.filterForm.value,
      pageNumber: this.pageNumber,
      pageSize: this.pageSize
    })
      .subscribe((result: any) => {
        this.inventories = result.data;
        this.totalCount = result.totalCount;
      });
  }
  onPageChange(event: any) {
    this.pageNumber = event.pageIndex + 1; // pageIndex is 0 based
    this.pageSize = event.pageSize;
    this.loadInventory();
  }
// filter
  applyFilter() {
    const { minQuantity, maxQuantity } = this.filterForm.value;
    if (
      minQuantity !== null &&
      maxQuantity !== null &&
      minQuantity > maxQuantity
    ) {
      this.showMessage(
        'Min quantity cannot be greater than max quantity',
        true
      );
    }
    this.pageNumber = 1;
    this.loadInventory();
  }

  clearFilter() {
    this.filterForm.reset({
      productId: null,
      minQuantity: null,
      maxQuantity: null,
      productName: '',
      isLowStock: false
    });
    this.pageNumber = 1;
    this.loadInventory();
  }
  // create/update
 /* saveInventory() {
    if (this.inventoryForm.invalid) return;

    const dto = {
      productId: this.inventoryForm.value.productId,
      quantityInStock: this.inventoryForm.value.quantity
    };

    this.loading = true;

    if (this.editingId === null) {
      // create
      this.inventoryService.createInventory(dto).subscribe({
        next: () => {
          this.showMessage('Inventory successfully created');
          this.loadInventory();
          this.inventoryForm.reset();
          this.loading = false;
        },
        error: (err) => {
          this.showMessage(err.error?.error || 'Error creating inventory', true);
          this.loading = false;
        }
      });

    } else {
      // update
      this.inventoryService.updateInventory(dto.productId, dto).subscribe({
        next: () => {
          this.showMessage('Inventory successfully updated');
          this.loadInventory();
          this.inventoryForm.reset();
          this.editingId = null;
          this.loading = false;
        },
        error: (err) => {
          this.showMessage(err.error?.error || 'Error updating inventory', true);
          this.loading = false;
        }
      });
    }
  } */

  submitWizard() {
    if (this.productStepForm.invalid || this.inventoryStepForm.invalid) return;

    const dto = {
      productId: this.productStepForm.value.productId,
      quantityInStock: this.inventoryStepForm.value.quantity
    };

    this.loading = true;

    // ADD
    if (this.editingId === null) {
      this.inventoryService.createInventory(dto).subscribe({
        next: () => {
          this.showMessage('Inventory successfully created');
          this.afterSave();
        },
        error: () => {
          this.showMessage('Error creating inventory', true);
          this.loading = false;
        }
      });
    }
    // EDIT
    else {
      this.inventoryService.updateInventory(this.editingId, dto).subscribe({
        next: () => {
          this.showMessage('Inventory successfully updated');
          this.afterSave();
        },
        error: () => {
          this.showMessage('Error updating inventory', true);
          this.loading = false;
        }
      });
    }
  }
  //helper m
  afterSave() {
    this.loadInventory();
    this.productStepForm.reset();
    this.inventoryStepForm.reset();
    this.editingId = null;
    this.loading = false;

    this.stepper.reset();
  }

  //edit
  edit(item: any) {
    this.editingId = item.productId;
  /*  this.inventoryForm.patchValue({
      productId: item.productId,
      quantity: item.quantity
    }); */

    this.productStepForm.patchValue({
      productId: item.productId
    });

    this.inventoryStepForm.patchValue({
      quantity: item.quantity
    });
  }
//delete
  delete(item: InventoryDto) {
    this.inventoryService.deleteInventory(item.productId).subscribe({
      next: () => {
        this.showMessage('Inventory deleted successfully');
        this.loadInventory();
      },
      error: () => {
        this.showMessage('Error deleting inventory', true);
      }
    });
  }
  displayedColumns: string[] = [
    'productName',
    'quantity',
    'lastUpdated',
    'actions'
  ];
  get completionPercent(): number {
    if (!this.stepper) return 0;

    const totalSteps = this.stepper.steps.length;
    const completedSteps = this.stepper.selectedIndex;

    return Math.round((completedSteps / totalSteps) * 100);
  }
logout() {
  this.auth.logout();
  this.router.navigate(['/']);
}
}
