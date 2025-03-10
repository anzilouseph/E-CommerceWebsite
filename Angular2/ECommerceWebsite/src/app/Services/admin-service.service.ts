import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProductListingMOdel } from '../Models/ProductToListModel';
@Injectable({
  providedIn: 'root'
})
export class AdminServiceService {

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



    // ALL THESE ARE FOR USER MANAGEMENT

    //for AddUser By Admin her can create either Admin or a user
    AddUserByAdminServiceFn(data:any)
    {
      return this.Http.post(`${this.baseUrl}AdminManagement/AddUserByAdmin`,data,this.getHeaders());
    }

    //for Get All Users Only Admin Can Access It
    GetAllAdminServiceFn()
    {
      return this.Http.get(`${this.baseUrl}AdminManagement/GetAllUsers`,this.getHeaders());
    }

    //for get the user by the admin
    GetUserByAdminServiceFn(id:any)
    {
      return this.Http.get(`${this.baseUrl}AdminManagement/GetUser?id=${id}`,this.getHeaders());
    }


    //Get a single user or a set of users by the name
    GetByNameAdminServiceFn(name:any)
    {
      return this.Http.get(`${this.baseUrl}AdminManagement/GetByName?name=${name}`,this.getHeaders());
    }

    //get the count of all users
    GettUserCount()
    {
      return this.Http.get(`${this.baseUrl}AdminManagement/UsersCount`,this.getHeaders());
    }

     //For Delete a user only admn can access
     DeleteUser(id:any)
     {
       return this.Http.delete(`${this.baseUrl}AdminManagement/DeleteUser?id=${id}`,this.getHeaders());
     }



     //PRODUCT MANAGEMENT FUNCTIONALITIES

     //for Add Product
     AddProductService(data:any)
     {
       return this.Http.post(`${this.baseUrl}AdminManagement/AddProduct`,data,this.getHeaders());
     }

   
     //for get the category Id by the Category name
     GetCategoryId(categoryName:any)
     {
      return this.Http.get(`${this.baseUrl}AdminManagement/GetCategoryId?categoryName=${categoryName}`,this.getHeaders());
     }

     //for get all products
     GetAllProducts() : Observable<ProductListingMOdel[]>
     {
      return this.Http.get<ProductListingMOdel[]>(`${this.baseUrl}AdminManagement/GetAllProducts`,this.getHeaders());
     }

     //for get the Image
      GetProductImage(productName:any)
      {
        return this.Http.get(`${this.baseUrl}AdminManagement/Product_image?fileName=${productName}`,
           {
              // headers: new HttpHeaders({
              //     'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
              // }), 
              responseType: 'blob' // Ensures we get the image as a binary blob
           });
      }


      //for get the Product by te catgeory name
      GetProductsByCategory(categoryName:any) : Observable<ProductListingMOdel[]>
      {
        return this.Http.get<ProductListingMOdel[]>(`${this.baseUrl}AdminManagement/GetProductByCategory?categoryName=${categoryName}`);
      }

      // for get the product by the id of that product
      GetProductById(id:any)
      {
        return this.Http.get(`${this.baseUrl}AdminManagement/GetProductById?id=${id}`,this.getHeaders());
      }

      //for update the proct
      UpdateProdcutService(id:any,data:any)
      {
        return this.Http.put(`${this.baseUrl}AdminManagement/UpdateProduct?product_id=${id}`,data,this.getHeaders());
      }

      //For get the total  count of products
      GetProductCountFn()
      {
        return this.Http.get(`${this.baseUrl}AdminManagement/GetProductCount`,this.getHeaders());
      }

      //for search product
      SearchProduct(pname:string) :Observable<ProductListingMOdel[]>
      {
        return this.Http.get<ProductListingMOdel[]>(`${this.baseUrl}AdminManagement/SearchProduct?pname=${pname}`)
      }

    }
