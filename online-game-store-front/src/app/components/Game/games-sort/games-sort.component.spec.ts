import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GamesSortComponent } from './games-sort.component';

describe('GamesSortComponent', () => {
  let component: GamesSortComponent;
  let fixture: ComponentFixture<GamesSortComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GamesSortComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GamesSortComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
