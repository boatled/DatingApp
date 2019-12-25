import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root' // The service is injectable only on the root component.
                     // The correspondent entry is made in app.module.ts,
                     // where we import the AuthModule under 'providers'
})
export class AuthService {
  baseUrl = 'http://localhost:5000/api/auth/'; // Specify the Base URL

  constructor(private http: HttpClient) {} // Creating 'http' variable of type HttpClient to perform
                                           // GET, PUSH, PUT, DELETE and PATCH operations.

  // The login method is responsible to pass model value and return the token.
  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model).pipe(
      map((response: any) => {
        const user = response; // Assign the value of response to constant variable 'user'
        if (user) {
          localStorage.setItem('token', user.token); // Set the token as localStorage
        }
      }) // Chaining the rsjx operators. The version of rsjx corresponds with the Angular version.
    );
  }

  // Responsble to authenticate the 'register' request
  // The method returns an Observable and would need to subscribe before using it.
  register(model: any){
    return this.http.post(this.baseUrl + 'register', model);
  }
}
