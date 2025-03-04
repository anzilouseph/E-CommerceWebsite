import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserDashboardCompComponent } from './user-dashboard-comp.component';

describe('UserDashboardCompComponent', () => {
  let component: UserDashboardCompComponent;
  let fixture: ComponentFixture<UserDashboardCompComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserDashboardCompComponent]
    });
    fixture = TestBed.createComponent(UserDashboardCompComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
