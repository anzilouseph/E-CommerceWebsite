import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WishListCompComponent } from './wish-list-comp.component';

describe('WishListCompComponent', () => {
  let component: WishListCompComponent;
  let fixture: ComponentFixture<WishListCompComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WishListCompComponent]
    });
    fixture = TestBed.createComponent(WishListCompComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
