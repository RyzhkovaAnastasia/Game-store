import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublisherCreatorComponent } from './publisher-creator.component';

describe('PublisherCreatorComponent', () => {
  let component: PublisherCreatorComponent;
  let fixture: ComponentFixture<PublisherCreatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PublisherCreatorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PublisherCreatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
