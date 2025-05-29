import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubjectEnrollmentComponent } from './subject-enrollment.component';

describe('SubjectEnrollmentComponent', () => {
  let component: SubjectEnrollmentComponent;
  let fixture: ComponentFixture<SubjectEnrollmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SubjectEnrollmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SubjectEnrollmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
