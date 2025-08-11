import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScopeStyling } from './scope-styling';

describe('ScopeStyling', () => {
  let component: ScopeStyling;
  let fixture: ComponentFixture<ScopeStyling>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ScopeStyling]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ScopeStyling);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
