import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GetCategoryModel } from 'src/app/Models/GetCategoryModel';
import { ProductListingMOdel } from 'src/app/Models/ProductToListModel';
import { AdminServiceService } from 'src/app/Services/admin-service.service';
import { CommonServicesService } from 'src/app/Services/common-services.service'; 

@Component({
  selector: 'app-user-dashboard-comp',
  templateUrl: './user-dashboard-comp.component.html',
  styleUrls: ['./user-dashboard-comp.component.css']
})
export class UserDashboardCompComponent {
  
  constructor(private router:Router,private commonService:CommonServicesService,private AService:AdminServiceService){}

  productList :ProductListingMOdel[]=[];
  errorMessage:any;

  ngOnInit()
  {
    this.GetAllProduct();
  }
  GetAllProduct()
  {
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
 
    
}


