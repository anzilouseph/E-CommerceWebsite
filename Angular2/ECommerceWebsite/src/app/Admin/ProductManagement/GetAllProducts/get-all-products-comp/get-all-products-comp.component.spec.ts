import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetAllProductsCompComponent } from './get-all-products-comp.component';

describe('GetAllProductsCompComponent', () => {
  let component: GetAllProductsCompComponent;
  let fixture: ComponentFixture<GetAllProductsCompComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GetAllProductsCompComponent]
    });
    fixture = TestBed.createComponent(GetAllProductsCompComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
