import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CartCompComponent } from './cart-comp.component';

describe('CartCompComponent', () => {
  let component: CartCompComponent;
  let fixture: ComponentFixture<CartCompComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CartCompComponent]
    });
    fixture = TestBed.createComponent(CartCompComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
