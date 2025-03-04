import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserLayoutCompComponent } from './user-layout-comp.component';

describe('UserLayoutCompComponent', () => {
  let component: UserLayoutCompComponent;
  let fixture: ComponentFixture<UserLayoutCompComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserLayoutCompComponent]
    });
    fixture = TestBed.createComponent(UserLayoutCompComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
