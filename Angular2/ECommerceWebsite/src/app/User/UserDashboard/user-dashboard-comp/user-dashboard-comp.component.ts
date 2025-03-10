import { Component } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { GetCategoryModel } from 'src/app/Models/GetCategoryModel';
import { ProductListingMOdel } from 'src/app/Models/ProductToListModel';
import { AdminServiceService } from 'src/app/Services/admin-service.service';
import { CommonServicesService } from 'src/app/Services/common-services.service'; 
import { UserServiceService } from 'src/app/Services/user-service.service';
import { SharedServicesService } from '../../SharedServices/shared-services.service';
import Swal from 'sweetalert2';
import { UserLayoutCompComponent } from '../../UserLayout/user-layout-comp/user-layout-comp.component';

@Component({
  selector: 'app-user-dashboard-comp',
  templateUrl: './user-dashboard-comp.component.html',
  styleUrls: ['./user-dashboard-comp.component.css']
})
export class UserDashboardCompComponent {
  
  constructor(private router:Router,private commonService:CommonServicesService,private AService:AdminServiceService,private UService:UserServiceService,private sharedStateService: SharedServicesService,private userlayout:UserLayoutCompComponent){
  }

  
  errorMessage:any;
  subscription:any;
  search:boolean=false;
  ngOnInit()
  {
    // Listen for reset state events
    this.sharedStateService.resetState$.subscribe(() => {
      this.resetState();
    });


    //for the searchproduct by name
    // Subscribe to the shared service's observable
    this.subscription = this.sharedStateService.buttonClicked$.subscribe((data) => {
      this.GetProductsByName(data); // Call the function when data is received
    });


    this.GetCategories();
    this.GetAllProduct();
    this.search=false;

  }


  filtered:boolean=false;

  CateorgyNames :GetCategoryModel [] = [] ;
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


  productList :ProductListingMOdel[]=[];
  //this fn is to get all the products
  GetAllProduct()
  {
    this.search=false;
    this.filtered=false;
    this.AService.GetAllProducts().subscribe({
                                              next:(res:any)=>
                                                {
                                                  console.log(res);
                                                  
                                                  if(res.status)
                                                    {
                                                      this.productList = res.data.map((product:any)=>
                                                                                      ({
                                                                                        ...product,  // ✅ Copy existing product properties
                                                                                        Original_Image:"" // ✅ Initialize an empty field for the image
                                                                                      })
                                                                                    );
                                                                                    console.log(this.productList);
                                                                                    

                                                                                    this.productList.forEach((product)=>
                                                                                                                      {
                                                                                                                        const fileName = product.urlOfImage.split('/').pop();
                                                                                                                        this.AService.GetProductImage(fileName).subscribe({
                                                                                                                                                                            next:(res1:any)=>
                                                                                                                                                                                            {
                                                                                                                                                                                              console.log(`Blob received for ${product.idOfProduct}:`, res1);
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

                                                                                                                      }
                                                                                                             );
                                                                                    

                                                                                      
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
 
   
  

  //to add an Item to Wishlist          
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
          this.userlayout.GetCountOfWishlist();
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


filteredProductList : ProductListingMOdel [] = [] ;
GetProductsByCategory(id:any) //this fn is to filter the products based on the category
{
  console.log("this fn is cALLED");
  console.log(id);
  
  
  this.filtered=true;
  this.filteredProductList = this.productList.filter(product=> product.idOfCategory === id);
  console.log(this.filteredProductList);
  
}


resetState()//for resetting the filter back to fasle and make the filteredproductlist as null
{
  this.filtered = false;
  this.filteredProductList = [];
}
  

  navigateToProduct(id:any)  //for making the whole card clickable
  {
    this.router.navigate(['/UserLayout/getProductByIdUser',id]) // to navigate to the GetProductById
  }



  //for add an item to the cart
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
          this.userlayout.GetCountOfCart();
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



  //for Filtering the products by the name
  searchedList:ProductListingMOdel[] =[];
  emessage:any;
  
  GetProductsByName(data:string)
  {
    this.search=true;
    this.AService.SearchProduct(data).subscribe({
      next:(res:any)=>
      {
        if(res.status)
          {
            this.searchedList = res.data.map((product:any)=>
                                            ({
                                              ...product,  // ✅ Copy existing product properties
                                              Original_Image:"" // ✅ Initialize an empty field for the image
                                            })
                                          );
                                          console.log(this.searchedList);
                                          

                                          this.searchedList.forEach((product)=>
                                                                            {
                                                                              const fileName = product.urlOfImage.split('/').pop();
                                                                              this.AService.GetProductImage(fileName).subscribe({
                                                                                                                                  next:(res1:any)=>
                                                                                                                                                  {
                                                                                                                                                    console.log(`Blob received for ${product.idOfProduct}:`, res1);
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

                                                                            }
                                                                   );
                                          

                                            
          }
          else
          {
            this.emessage=res.message;
            console.log(res);           
          }
      },
      error:error=>
      {
        this.emessage=error.message;
            console.log(error);          
      }
    });
  }


}


