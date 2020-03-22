import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'vid-nav-menu-video',
  templateUrl: './nav-menu-video.component.html',
  styleUrls: ['./nav-menu-video.component.css']
})
export class NavMenuVideoComponent implements OnInit {

  isExpanded = false;

  constructor() { }

  ngOnInit() {
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
