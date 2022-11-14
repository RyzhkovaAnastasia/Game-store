import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenreCreatorComponent } from './genre-creator.component';

describe('GenreCreatorComponent', () => {
  let component: GenreCreatorComponent;
  let fixture: ComponentFixture<GenreCreatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GenreCreatorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GenreCreatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
