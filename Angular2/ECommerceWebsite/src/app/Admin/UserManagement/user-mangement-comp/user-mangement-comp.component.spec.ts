import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserMangementCompComponent } from './user-mangement-comp.component';

describe('UserMangementCompComponent', () => {
  let component: UserMangementCompComponent;
  let fixture: ComponentFixture<UserMangementCompComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserMangementCompComponent]
    });
    fixture = TestBed.createComponent(UserMangementCompComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
