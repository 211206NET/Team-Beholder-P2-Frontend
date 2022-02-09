import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {

  constructor() { }

  proccessForm(userForm:NgForm)
  {
    console.log('form has been submitted')
  }

  ngOnInit(): void {
  }

}
