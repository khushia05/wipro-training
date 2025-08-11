import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StyledCard } from './styled-card';

describe('StyledCard', () => {
  let component: StyledCard;
  let fixture: ComponentFixture<StyledCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StyledCard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StyledCard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
