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
import {RouterLink} from '@angular/router';
import {AuthService} from '../../core/services/auth/auth.service';
import {Router, RouterModule} from '@angular/router';
import { MatPaginatorModule } from '@angular/material/paginator';


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
    MatButtonModule,
    MatPaginatorModule,
    RouterModule, RouterLink],
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css']
})
export class ShopComponent {

  carouselProducts: ProductDto[] = [];
  products: any[] = [];
  totalCount = 0;

  currentSearch: string = '';

  pageNumber = 1;
  pageSize = 9;

  searchControl = new FormControl('');

  constructor(private shopService: ShopService,
              public auth: AuthService,
              private router: Router,
              ) {

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
    this.currentSearch = search;

    this.shopService.getProducts({
      search,
      pageNumber: this.pageNumber,
      pageSize: this.pageSize

    }).subscribe(res => {
      this.products = res.data;
      this.totalCount = res.totalCount;

      if (this.carouselProducts.length === 0) {
        this.carouselProducts = [...res.data]
          .sort((a, b) => a.price - b.price)
          .slice(0, 8);
      }
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
  logout() {
    this.auth.logout();
    this.router.navigate(['/']);
  }
  onPageChange(event: any) {
    this.pageNumber = event.pageIndex + 1; // Angular paginator je 0-based
    this.pageSize = event.pageSize;
    this.loadProducts(this.currentSearch);
  }
}
