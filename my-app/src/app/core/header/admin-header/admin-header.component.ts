import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'admin-header',
  templateUrl: './admin-header.component.html',
  styleUrls: ['./admin-header.component.css']
})
export class AdminHeaderComponent implements OnInit {

  constructor(private router : Router) { }

  ngOnInit() { }

  Logout() {
    localStorage.removeItem('userToken');
    localStorage.removeItem('role');
    this.router.navigate(['/login']);
  }

}
