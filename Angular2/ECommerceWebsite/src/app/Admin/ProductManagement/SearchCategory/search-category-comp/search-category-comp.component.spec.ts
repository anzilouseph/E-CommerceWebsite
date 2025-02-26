import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchCategoryCompComponent } from './search-category-comp.component';

describe('SearchCategoryCompComponent', () => {
  let component: SearchCategoryCompComponent;
  let fixture: ComponentFixture<SearchCategoryCompComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SearchCategoryCompComponent]
    });
    fixture = TestBed.createComponent(SearchCategoryCompComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
