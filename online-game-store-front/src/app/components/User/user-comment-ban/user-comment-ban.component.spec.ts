import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserCommentBanComponent } from './user-comment-ban.component';

describe('UserCommentBanComponent', () => {
  let component: UserCommentBanComponent;
  let fixture: ComponentFixture<UserCommentBanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserCommentBanComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserCommentBanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
