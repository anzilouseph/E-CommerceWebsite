import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchUserCompComponent } from './search-user-comp.component';

describe('SearchUserCompComponent', () => {
  let component: SearchUserCompComponent;
  let fixture: ComponentFixture<SearchUserCompComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SearchUserCompComponent]
    });
    fixture = TestBed.createComponent(SearchUserCompComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
