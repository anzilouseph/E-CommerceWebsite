import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductListingMOdel } from 'src/app/Models/ProductToListModel';
import { AdminServiceService } from 'src/app/Services/admin-service.service';
import { UserServiceService } from 'src/app/Services/user-service.service';
import Swal from 'sweetalert2';
@Component({
  selector: 'app-get-product-by-id-user',
  templateUrl: './get-product-by-id-user.component.html',
  styleUrls: ['./get-product-by-id-user.component.css']
})
export class GetProductByIdUserComponent {

   constructor(private AService:AdminServiceService,private route:ActivatedRoute,private UService:UserServiceService){}
  
    productList : ProductListingMOdel | null = null;
    errorMessage:any;
    id:any;
    quantity:number=1;

    ngOnInit()
    {
      this.GetProductById();
    }
  
    GetProductById()
    {
      this.id = this.route.snapshot.params["id"];
      this.AService.GetProductById(this.id).subscribe({
                                                        next:(res:any)=>
                                                        {
                                                          if(res.status)
                                                          {
                                                            console.log("product get succesfully but this product only have the imagename not the original image");
                                                            this.productList = {...res.data,Original_Image:""};
                                                            
                                                              const fileName = this.productList?.urlOfImage.split('/').pop();
                                                              this.AService.GetProductImage(fileName).subscribe({
                                                                                                                  next:(res1:any)=>
                                                                                                                  {
                                                                                                                    console.log("Image get successfully");
  
                                                                                                                    const objUrl = URL.createObjectURL(res1);
                                                                                                                    this.productList!.Original_Image = objUrl;
                                                                                                                    
                                                                                                                  },
                                                                                                                  error:error=>
                                                                                                                  {
                                                                                                                    this.errorMessage=error.message;
                                                                                                                    console.log(error);    
                                                                                                                  }
                                                                                                                 })
                                                          }
                                                          else
                                                          {
                                                            console.log("the product is not get");
                                                            this.errorMessage=res.message;        
                                                            
                                                          }
                                                        },
                                                        error:error=>
                                                        {
                                                          console.log("it comes to the error message of getProduct ByID")
                                                          this.errorMessage=error.message;
                                                        }
                                                      });
    }

     //to increase the quantity
     increaseQuantity()
     {
       if(this.quantity<10)
       {
         this.quantity++;
       }
       else
       {
         alert("Only order 10 At a time")
       }
     }
     
     //to decrease
     decreaseQuantity()
     {
       if(this.quantity>1)
       {
         this.quantity--;
       }
     }
 
 
   //to add an Item to Wishlist
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
  AddWishList(pid:any)
  {
    const dto = {"idOfProduct":pid};
    this.UService.AddToWishList(dto).subscribe({
      next:(res:any)=>
      {
        if(res.status)
        {
          console.log(res.data);
          alert(res.message);
        }
        else
        {
          console.log(res);  
          alert(res.message);
        }
      },
      error:error=>
      {
        console.log(error);      
        alert(error.message);
      }
    });
  }  


  //for Add the product to the wishlist
  AddProductToCart(id:any)
  {
    const dto={"idOfProduct":id}
    this.UService.AddToCart(dto).subscribe({
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
