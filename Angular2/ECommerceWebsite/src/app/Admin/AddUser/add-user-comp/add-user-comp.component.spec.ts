import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUserCompComponent } from './add-user-comp.component';

describe('AddUserCompComponent', () => {
  let component: AddUserCompComponent;
  let fixture: ComponentFixture<AddUserCompComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddUserCompComponent]
    });
    fixture = TestBed.createComponent(AddUserCompComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
