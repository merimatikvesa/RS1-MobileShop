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
    MatTableModule
  ],
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css']
})
export class InventoryComponent implements OnInit {

  inventories: InventoryDto[] = [];
  //two forms
  filterForm!: FormGroup;
  inventoryForm!: FormGroup;

  editingId: number | null = null;

  loading = false;

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
    this.inventoryForm = this.fb.group({
      productId: [null, Validators.required],
      quantity: [null, [Validators.required, Validators.min(0)]]
    });

    this.loadInventory();
  }
// read
  loadInventory() {
    this.inventoryService.getInventory(this.filterForm.value)
      .subscribe(result => this.inventories = result);
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

    this.loadInventory();
  }
  // create/update
  saveInventory() {
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
  }
  //edit
  edit(item: any) {
    this.editingId = item.productId;
    this.inventoryForm.patchValue({
      productId: item.productId,
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
}
