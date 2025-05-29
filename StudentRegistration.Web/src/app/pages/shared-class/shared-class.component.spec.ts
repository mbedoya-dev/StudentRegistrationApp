import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SharedClassComponent } from './shared-class.component';

describe('SharedClassComponent', () => {
  let component: SharedClassComponent;
  let fixture: ComponentFixture<SharedClassComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SharedClassComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SharedClassComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
