import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetProductByIdUserComponent } from './get-product-by-id-user.component';

describe('GetProductByIdUserComponent', () => {
  let component: GetProductByIdUserComponent;
  let fixture: ComponentFixture<GetProductByIdUserComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GetProductByIdUserComponent]
    });
    fixture = TestBed.createComponent(GetProductByIdUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
