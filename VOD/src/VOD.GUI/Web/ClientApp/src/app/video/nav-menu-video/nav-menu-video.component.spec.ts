import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NavMenuVideoComponent } from './nav-menu-video.component';

describe('NavMenuVideoComponent', () => {
  let component: NavMenuVideoComponent;
  let fixture: ComponentFixture<NavMenuVideoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NavMenuVideoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NavMenuVideoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
