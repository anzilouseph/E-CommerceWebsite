import { Component } from '@angular/core';
import { AdminServiceService } from 'src/app/Services/admin-service.service';
import { ActivatedRoute } from '@angular/router';
import { ProductListingMOdel } from 'src/app/Models/ProductToListModel';
import { FormGroup,FormControl,Validators } from '@angular/forms';
@Component({
  selector: 'app-get-product-by-id-comp',
  templateUrl: './get-product-by-id-comp.component.html',
  styleUrls: ['./get-product-by-id-comp.component.css']
})
export class GetProductByIdCompComponent {

  constructor(private service:AdminServiceService,private route:ActivatedRoute){}

  productModel :ProductListingMOdel | null = null;


  id:any;
  errorMessage:any;

  ngOnInit()
  {   
    this.getProductById(); 
  }

  getProductById()   // this  fn is to the Single porduct by the id
  {
    this.id = this.route.snapshot.paramMap.get("id");
    this.service.GetProductById(this.id).subscribe(
      {
        next:(res:any)=>
        {
          if(res.status)
          {
            console.log("product get succesfully but this product only have the imagename not the original image");
            this.productModel={...res.data,Original_Image:""};
             
            const fileName = this.productModel?.urlOfImage.split('/').pop();
            
            this.service.GetProductImage(fileName).subscribe({
                                                            next:(res1:any)=>
                                                              {
                                                               
                                                                console.log("Image loaded.");
                                                                const objUrl = URL.createObjectURL(res1);
                                                                this.productModel!.Original_Image = objUrl;
                                                              },
                                                              error:error=>
                                                              {
                                                                this.errorMessage=error.message;
                                                                console.log(error);                                                                
                                                              }
                                                           });
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





  // for updating the product functinalitys STARTS HERE

  updateForm = new FormGroup(
    {
      nameOfProduct : new FormControl("",Validators.required),
      descriptionOfProduct : new FormControl(""),
      priceOfProduct : new FormControl("",[Validators.required,Validators.pattern(/^\d+(\.\d{1,2})?$/)]),
      availableQuantity : new FormControl("",[Validators.required,  Validators.pattern(/^(100|[1-9]?[0-9])$/)]),
      idOfCategory : new FormControl("",Validators.required),  
    }
  );


  selectedFile :File | null = null;

  onFileSelected(event:any)
  {
    const file = event.target.files[0];  //here we are storing the image to variable file
    if(file)
    {
      this.selectedFile=file;  // we are storing that image into the variabke selected file;
    } 
  }


  isEdit:boolean=false;
  onEdit()
  {
    this.isEdit=true;
    this.updateForm.patchValue(
      {
        nameOfProduct : this.productModel?.nameOfProduct,
        descriptionOfProduct: this.productModel?.descriptionOfProduct,
        priceOfProduct : this.productModel?.priceOfProduct.toString(),
        availableQuantity: this.productModel?.availableQuantity.toString(),
      }
    )
  }


  updateFormData:any;

  updateProduct()
  {
    if(this.updateForm.valid)
    {
      const formData = new FormData();
      formData.append("nameOfProduct",this.updateForm.value.nameOfProduct ?? "");
      formData.append("descriptionOfProduct",this.updateForm.value.descriptionOfProduct ?? "");
      formData.append("priceOfProduct",this.updateForm.value.priceOfProduct ?? ""); //added price
      formData.append("availableQuantity",this.updateForm.value.availableQuantity ?? ""); //added Quantity
      
      if(this.selectedFile)
      {
        formData.append("product_image",this.selectedFile,this.selectedFile.name);
      }

      this.service.GetCategoryId(this.updateForm.value.idOfCategory).subscribe({
                                                                                  next:(res:any)=>
                                                                                    {
                                                                                      if(res.status)
                                                                                      {
                                                                                        formData.append("idOfCategory",res.data);
                                                                                        this.service.UpdateProdcutService(this.id,formData).subscribe({
                                                                                                                                                        next:(res1:any)=>
                                                                                                                                                          {
                                                                                                                                                            if(res1.status)
                                                                                                                                                            {
                                                                                                                                                              alert(res1.message);
                                                                                                                                                              console.log(res1);
                                                                                                                                                              this.getProductById();
                                                                                                                                                              this.isEdit=false;
                                                                                                                                                            }
                                                                                                                                                            else
                                                                                                                                                            {
                                                                                                                                                              alert(res.message)
                                                                                                                                                              console.log(res1);
                                                                                                                                                              this.isEdit=false;                                                                                                                                                                 
                                                                                                                                                            }
                                                                                                                                                          },
                                                                                                                                                          error:error=>
                                                                                                                                                            {
                                                                                                                                                             
                                                                                                                                                              console.log(error);
                                                                                                                                                              this.errorMessage=error.message
                                                                                                                                                            }     
                                                                                                                                                        });
                                                                                      }
                                                                                      else
                                                                                      {
                                                                                        console.log(res);
                                                                                        this.errorMessage=res.message
                                                                                      }
                                                                                    },
                                                                                    error:error=>
                                                                                      {
                                                                                        alert(error.message)
                                                                                        console.log(error);
                                                                                        this.errorMessage=error.message
                                                                                        this.isEdit=false
                                                                                      }        
                                                                                });

    }
    else
    {
      alert("invalid form format")
    }
  }

  reset()  //for resetting the update
  {
    this.updateForm.patchValue(
      {
        nameOfProduct : this.productModel?.nameOfProduct,
        descriptionOfProduct: this.productModel?.descriptionOfProduct,
        priceOfProduct : this.productModel?.priceOfProduct.toString(),
        availableQuantity: this.productModel?.availableQuantity.toString(),
      }
    )
  }
  goback() //for gi back from the updating page
  {
    this.isEdit=false;
  }
}
