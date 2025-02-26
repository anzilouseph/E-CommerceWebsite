import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class UserServiceService {

  constructor(private Http:HttpClient) { }

  
  private getHeaders() {
    const token = localStorage.getItem('accessToken'); // Retrieve token dynamically
    return {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      }) 
    };
  }

  baseUrl="https://localhost:7165/api/";

  //for AddUser
  UserRegistrationServiceFn(data:any)
  {
    return this.Http.post(`${this.baseUrl}AddUser`,data);
  }

  GetOwnProfileServiceFn()
  {
    return this.Http.get(`${this.baseUrl}GetUserOwnProfile`,this.getHeaders());
  }

}
