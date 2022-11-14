import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GamesPaginationComponent } from './games-pagination.component';

describe('GamesPaginationComponent', () => {
  let component: GamesPaginationComponent;
  let fixture: ComponentFixture<GamesPaginationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GamesPaginationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GamesPaginationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
