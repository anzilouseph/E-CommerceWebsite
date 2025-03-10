import { Component } from '@angular/core';
import { UserServiceService } from 'src/app/Services/user-service.service';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { ProductListingMOdel } from 'src/app/Models/ProductToListModel';
import { AdminServiceService } from 'src/app/Services/admin-service.service';
import {  ToastrService } from 'ngx-toastr'; // Import ToastrService
import Swal from 'sweetalert2';
import { UserLayoutCompComponent } from '../../UserLayout/user-layout-comp/user-layout-comp.component';

@Component({
  selector: 'app-wish-list-comp',
  templateUrl: './wish-list-comp.component.html',
  styleUrls: ['./wish-list-comp.component.css']
})
export class WishListCompComponent {

  constructor(private userService:UserServiceService, private AService:AdminServiceService,private toastr:ToastrService,private router:Router,private userlayout:UserLayoutCompComponent){}

  productModel : ProductListingMOdel[] = [];
  errorMessage :any;

  
  Toast = Swal.mixin({  //for toast
    toast: true,
    position: 'top-right',
    iconColor: 'white',
    customClass: {
      popup: 'colored-toast',
    },
    showConfirmButton: false,
    timer: 1500,
    timerProgressBar: true,
  });


  ngOnInit()
  {
    this.ShowWishList();
   
  }


  
  ShowWishList()
  {
    this.userService.GetProductsFromWatchList().subscribe({
      next:(res:any)=>
      {
        if(res.status)
        {
          console.log(res);
          this.productModel = res.data.map((product:any)=>
                                              ({
                                                ...product,
                                                Original_Image : "",
                                              })
                                            );
          this.productModel.forEach((product)=>
                                            {
                                              const fileName = product.urlOfImage.split("/").pop();
                                              this.AService.GetProductImage(fileName).subscribe({
                                                                                                  next:(res1:any)=>
                                                                                                    {
                                                                                                      console.log("image recieved ");
                                                                                                      const objUrl = URL.createObjectURL(res1);
                                                                                                      product.Original_Image = objUrl;
                                                                                                      console.log(`Image loaded for: ${product.idOfProduct}`, objUrl);
                                                                                                    },
                                                                                                    error:error=>
                                                                                                    {
                                                                                                      console.log(`Error fetching image for product: ${product.idOfProduct}`, error);
                                                                                                      product.Original_Image = undefined;
                                                                                                    }
                                                                                                  });
                                            });
        }
        else
        {
          this.errorMessage = res.message
          console.log(res); 
        }
      },
      error:error=>
        {
          console.log(error);
          this.errorMessage=error.message;        
        }
    });
  }


  //delete from wishlist
  DeleteFromWishList(pid:any)
  {
    this.userService.DeleteFromWishlist(pid).subscribe({
      next:(res:any)=>
      {
        if(res.status)
        {
          console.log(res);
          this.productModel = this.productModel.filter(product=>  product.idOfProduct!=pid);   // ✅ Remove the deleted product from the array manually,The filter() function goes through productModel and keeps only products that do NOT match the deleted pid.
          this.userlayout.GetCountOfWishlist();
          this.Toast.fire({
                            position:'bottom',
                            icon: 'success',
                            title: res.message,
                            background: 'rgba(0, 0, 0, 0.8)', // Black with slight transparency
                            color: 'white', // White text
                            toast: true,
                            showConfirmButton: false,
                            timerProgressBar: false,
                          })
            
          
        }
      else 
        {
        console.log(res);
        this.Toast.fire({
                            position:'bottom',
                            icon: 'error',
                            title: res.message,
                            background: 'rgba(0, 0, 0, 0.8)', // Black with slight transparency
                            color: 'white', // White text
                            toast: true,
                            showConfirmButton: false,
                            timerProgressBar: false,
                           })
        
         }
      },
      error:error=>
      {
        console.log(error);
        this.Toast.fire({
                            position:'bottom',
                            icon: 'error',
                            title: error.message,
                            background: 'rgba(0, 0, 0, 0.8)', // Black with slight transparency
                            color: 'white', // White text
                            toast: true,
                            showConfirmButton: false,
                            timerProgressBar: false,
                          })
      }
    });
  }


  navigateToProduct(id:any)  //for making the whole card clickable
  {
    this.router.navigate(['/UserLayout/getProductByIdUser',id]) // to navigate to the GetProductById
  }


  //for Add the product to the wishlist
  AddProductToCart(id:any)
  {
    const dto={"idOfProduct":id}
    this.userService.AddToCart(dto).subscribe({
      next:(res:any)=>
      {
        if(res.status)
        {
          console.log(res);
          
          this.Toast.fire({
                            position:'bottom',
                            icon: 'success',
                            title: res.message,
                            background: 'rgba(0, 0, 0, 0.8)', // Black with slight transparency
                            color: 'white', // White text
                            toast: true,
                            showConfirmButton: false,
                            timerProgressBar: false,
                          });
        }
        else
        {
          console.log(res);
          
          this.Toast.fire({
                            position:'bottom',
                            icon: 'error',
                            title: res.message,
                            background: 'rgba(0, 0, 0, 0.8)', // Black with slight transparency
                            color: 'white', // White text
                            toast: true,
                            showConfirmButton: false,
                            timerProgressBar: false,
                          });
        }
      },
      error:error=>
      {
        console.log(error);
        alert(error.message);
      }
    });
    
  }


}
