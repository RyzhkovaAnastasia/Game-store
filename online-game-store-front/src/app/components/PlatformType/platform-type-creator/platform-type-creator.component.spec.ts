import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlatformTypeCreatorComponent } from './platform-type-creator.component';

describe('PlatformTypeCreatorComponent', () => {
  let component: PlatformTypeCreatorComponent;
  let fixture: ComponentFixture<PlatformTypeCreatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlatformTypeCreatorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlatformTypeCreatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
