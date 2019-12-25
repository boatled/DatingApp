import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false; // Assign the registerMode to false
  values: any; // Assign values to this variable
  constructor(private http: HttpClient) { } // Declare the http in constructor to use http methods.

  ngOnInit() {
  }

  registerToggle(){
    this.registerMode = true; // A toggle switch over between register and home.
  }
  cancelRegisterMode(registerMode: boolean){
    this.registerMode = registerMode; // Change the registerMode value.
  }

}
