import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductCreateDialogComponent } from './product-create-dialog.component';

describe('ProductCreateDialogComponent', () => {
  let component: ProductCreateDialogComponent;
  let fixture: ComponentFixture<ProductCreateDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductCreateDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductCreateDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
