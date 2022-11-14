import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublisherEditorComponent } from './publisher-editor.component';

describe('PublisherEditorComponent', () => {
  let component: PublisherEditorComponent;
  let fixture: ComponentFixture<PublisherEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PublisherEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PublisherEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
