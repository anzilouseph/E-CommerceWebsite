import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileCompComponent } from './profile-comp.component';

describe('ProfileCompComponent', () => {
  let component: ProfileCompComponent;
  let fixture: ComponentFixture<ProfileCompComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProfileCompComponent]
    });
    fixture = TestBed.createComponent(ProfileCompComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
