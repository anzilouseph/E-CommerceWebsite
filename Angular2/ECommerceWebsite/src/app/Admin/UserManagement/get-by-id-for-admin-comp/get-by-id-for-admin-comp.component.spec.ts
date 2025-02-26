import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetByIdForAdminCompComponent } from './get-by-id-for-admin-comp.component';

describe('GetByIdForAdminCompComponent', () => {
  let component: GetByIdForAdminCompComponent;
  let fixture: ComponentFixture<GetByIdForAdminCompComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GetByIdForAdminCompComponent]
    });
    fixture = TestBed.createComponent(GetByIdForAdminCompComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
