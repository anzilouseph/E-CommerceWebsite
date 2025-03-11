import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CartProdcutsModel } from '../Models/CartProductModel';
import { UserToModel } from '../Models/UserModel';
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
        
      }) 
    };
  }

  baseUrl="https://localhost:7165/api/";

  //for AddUser
  UserRegistrationServiceFn(data:any)
  {
    return this.Http.post(`${this.baseUrl}AddUser`,data);
  }

  GetOwnProfileServiceFn() :Observable<UserToModel>
  {
    return this.Http.get<UserToModel>(`${this.baseUrl}GetUserOwnProfile`,this.getHeaders());
  }


  //to add an item to the wishlist
  AddToWishList(dto:any) 
  {
    return this.Http.post(`${this.baseUrl}Watchlist/AddToWishlist`,dto,this.getHeaders());
  }

  //for Get All The Products in the WatchList
  GetProductsFromWatchList()
  {
    return this.Http.get(`${this.baseUrl}Watchlist/WatchListProducts`,this.getHeaders());
  } 

  //for delete a product from teh wishlist
  DeleteFromWishlist(productId:any)
  {
    return this.Http.delete(`${this.baseUrl}Watchlist/DeleteFromWishlist?productid=${productId}`,this.getHeaders());
  }


  //for add a product to the cart
  AddToCart(dto:any)
  {
    return this.Http.post(`${this.baseUrl}Cart/AddToCart`,dto,this.getHeaders());
  }

  // Get all th eproducts from the CART
  GetProductsFromCart()  :Observable<CartProdcutsModel[]>
  {
    return this.Http.get<CartProdcutsModel[]>(`${this.baseUrl}Cart/GetCartProducts`,this.getHeaders());
  }

  //delete a product from the cart
  DeleteFromCart(ProductId:any)
  {
    return this.Http.delete(`${this.baseUrl}Cart/DeleteFromCart?productid=${ProductId}`,this.getHeaders());
  }


  //get the count of the products in the cart
  GetCartCount()
  {
    return this.Http.get(`${this.baseUrl}Cart/GetCartCount`,this.getHeaders());
  }

  //get the count of the products in the Wishlist
  GetWishlistCount()
  {
    return this.Http.get(`${this.baseUrl}Watchlist/GetWishlistCount`,this.getHeaders());
  }

  //FOR GET THE USER Profile Image
  GetProfileImage(filename:any)
  {
    return this.Http.get(`${this.baseUrl}User_Image?fileName=${filename}`,{responseType:'blob'});
  }

  //For Delete User Profile Image
  DeleteProfileImage()
  {
    return this.Http.delete(`${this.baseUrl}RemoveProfileImage`,this.getHeaders());
  }

  //FOR UPDATE PROFILE IMAGE
  updateProfileImage(data:any)
  {
    return this.Http.put(`${this.baseUrl}UpdateProfileImage`,data,this.getHeaders());
  }
  UpdateProfileDetails(data:any)
  {
    return this.Http.put(`${this.baseUrl}UpdateUser`,data,this.getHeaders());
  }

  }
