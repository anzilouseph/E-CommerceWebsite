import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GetCategoryModel } from '../Models/GetCategoryModel';
@Injectable({
  providedIn: 'root'
})
export class CommonServicesService {

  constructor(private Http:HttpClient) { }
  
 baseUrl:any="https://localhost:7165/api/"

  private getHeaders() {
      const token = localStorage.getItem('accessToken'); // Retrieve token dynamically
      return {
        headers: new HttpHeaders({
          'Authorization': `Bearer ${token}`,         
        }) 
      };
    }

    GetCategories() :Observable<GetCategoryModel[]>
    {
      return this.Http.get<GetCategoryModel[]>(`${this.baseUrl}GetCategories`);
    }


}
