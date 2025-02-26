import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchProductCompComponent } from './search-product-comp.component';

describe('SearchProductCompComponent', () => {
  let component: SearchProductCompComponent;
  let fixture: ComponentFixture<SearchProductCompComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SearchProductCompComponent]
    });
    fixture = TestBed.createComponent(SearchProductCompComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
