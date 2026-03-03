import { Component } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';
import { ShopService } from './shop.service';
import { CommonModule } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { ProductDto } from '../products/product.dto';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [ReactiveFormsModule,
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    MatSelectModule,
    MatGridListModule,
    MatIconModule,
    MatButtonModule],
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css']
})
export class ShopComponent {

  products: any[] = [];
  totalCount = 0;

  pageNumber = 1;
  pageSize = 9;

  searchControl = new FormControl('');

  constructor(private shopService: ShopService) {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(400),
        distinctUntilChanged()
      )
      .subscribe(value => {
        this.pageNumber = 1;
        this.loadProducts(value ?? '');
      });
  }

  ngOnInit() {
    this.loadProducts('');
  }

  loadProducts(search: string) {
    this.shopService.getProducts({
      search,
      pageNumber: this.pageNumber,
      pageSize: this.pageSize
    }).subscribe(res => {
      this.products = res.data;
      this.totalCount = res.totalCount;
    });
  }
  toggleFavorite(product: ProductDto) {
    let favs: number[] = JSON.parse(
      localStorage.getItem('favorites') || '[]'
    );

    if (favs.includes(product.productId)) {
      favs = favs.filter((id: number) => id !== product.productId);
    } else {
      favs.push(product.productId);
    }

    localStorage.setItem('favorites', JSON.stringify(favs));
  }
  isFavorite(productId: number): boolean {
    const favs: number[] = JSON.parse(
      localStorage.getItem('favorites') || '[]'
    );
    return favs.includes(productId);
  }
}
