import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetProductByCategoryCompComponent } from './get-product-by-category-comp.component';

describe('GetProductByCategoryCompComponent', () => {
  let component: GetProductByCategoryCompComponent;
  let fixture: ComponentFixture<GetProductByCategoryCompComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GetProductByCategoryCompComponent]
    });
    fixture = TestBed.createComponent(GetProductByCategoryCompComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
