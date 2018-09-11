import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'super-header',
  templateUrl: './super-header.component.html',
  styleUrls: ['./super-header.component.css']
})
export class SuperHeaderComponent implements OnInit {

  constructor(private router : Router) { }

  ngOnInit() { }

  Logout() {
    localStorage.removeItem('userToken');
    localStorage.removeItem('role');
    this.router.navigate(['/login']);
  }

}
