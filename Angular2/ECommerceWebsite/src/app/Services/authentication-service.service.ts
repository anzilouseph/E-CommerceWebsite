import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class AuthenticationServiceService {

  constructor(private Http:HttpClient) { }
  
  baseUrl="https://localhost:7165/api/";

  //For Login
  loginSericeFn(data:any)
  {
    return this.Http.post(`${this.baseUrl}Authentication/Login`,data)
  }
}
