import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetProductByIdCompComponent } from './get-product-by-id-comp.component';

describe('GetProductByIdCompComponent', () => {
  let component: GetProductByIdCompComponent;
  let fixture: ComponentFixture<GetProductByIdCompComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GetProductByIdCompComponent]
    });
    fixture = TestBed.createComponent(GetProductByIdCompComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
