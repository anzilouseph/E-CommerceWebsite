import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GetCategoryModel } from 'src/app/Models/GetCategoryModel';
import { AdminServiceService } from 'src/app/Services/admin-service.service';
import { CommonServicesService } from 'src/app/Services/common-services.service';
import { SharedServicesService } from '../../SharedServices/shared-services.service';
import { UserServiceService } from 'src/app/Services/user-service.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
@Component({
  selector: 'app-user-layout-comp',
  templateUrl: './user-layout-comp.component.html',
  styleUrls: ['./user-layout-comp.component.css']
})
export class UserLayoutCompComponent {

constructor(private router:Router,private Aservice:AdminServiceService,private commonService:CommonServicesService,private sharedservice:SharedServicesService,private uservice:UserServiceService){}
  userData:any
  ngOnInit()
  {
    if(localStorage.getItem("userData"))
    {
      this.userData=JSON.parse(localStorage.getItem("userData")!);
    }
    else
    {
      localStorage.clear;
      this.router.navigate(['']); //if their is no user data then logoutted
    }
    this. GetCategories();
    this.GetCountOfCart();
    this.GetCountOfWishlist();
  }

  CateorgyNames :GetCategoryModel [] = [] ;
  errorMessage :any;

//for get all the catergories
  GetCategories()
  {
    this.commonService.GetCategories().subscribe({
                                                  next:(res:any)=>
                                                    {
                                                      if(res.status)
                                                        {
                                                          this.CateorgyNames =res.data;
                                                          console.log(res);                                                          
                                                        }
                                                        else
                                                        {
                                                          this.errorMessage = res.message;
                                                          console.log(res);                                                        
                                                        }
                                                    },
                                                    error:error=>
                                                    {
                                                      this.errorMessage=error.message;
                                                      console.log(error);                                                     
                                                    }
                                                  });
  }



  // Method to trigger reset state
  onHomeClick() {
    this.sharedservice.triggerResetState();
  }


  //for get the total count of products in the cart
  countOfProduct:number = 0;
  GetCountOfCart()
  {
    this.uservice.GetCartCount().subscribe({
      next:(res:any)=>
      {
        if(res.status)
        {
          console.log(res);
          this.countOfProduct=res.data;          
        }
      },
      error:error=>
      {
        console.log(error);       
      }
    });
  }
  

   //for get the total count of products in the wishlist
   wishlistProducts:number = 0;
   GetCountOfWishlist()
   {
     this.uservice.GetWishlistCount().subscribe({
       next:(res:any)=>
       {
         if(res.status)
         {
           console.log(res);
           this.wishlistProducts=res.data;          
         }
       },
       error:error=>
       {
         console.log(error);       
       }
     });
   }
   



  // this form is for search a product
  searchForm = new FormGroup(
    {
      nameOfProduct : new FormControl("",[Validators.required]),
    }
  );

  //FOR EMIT THE SEARCHPRODUCT STARTS HERE
      // Method called when the button is clicked
      onButtonClick() {   
        const data = this.searchForm.get('nameOfProduct')?.value?.trim(); // ✅ Get input value correctly
        if (data) {
          this.sharedservice.emitButtonClick(data); // ✅ Send the value to the shared service
        } 
      }
//FOR EMIT THE SEARCHPRODUCT ENDS HERE


}
