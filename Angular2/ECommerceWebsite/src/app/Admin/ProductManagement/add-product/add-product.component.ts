import { Component } from '@angular/core';
import { AdminServiceService } from 'src/app/Services/admin-service.service';
import { FormControl, FormGroup,Validators} from '@angular/forms';
import { formatPercent } from '@angular/common';
import { Router } from '@angular/router';
import { SubscriptionLoggable } from 'rxjs/internal/testing/SubscriptionLoggable';
@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent {

  constructor(private service:AdminServiceService,private router:Router){}

  data:any;
  errorMessage:any;
  addProductFormData:any
  categoryId:any;
  changedCategoryId:any;

  addProductForm = new FormGroup(
    {
      nameOfProduct : new FormControl("",[Validators.required]),
      descriptionOfProduct : new FormControl(""),
      priceOfProduct : new FormControl("",[Validators.required,Validators.pattern(/^\d+(\.\d{1,2})?$/)]),
      availableQuantity : new FormControl("",[Validators.required,  Validators.pattern(/^(100|[1-9]?[0-9])$/)]),
      idOfCategory : new FormControl("",Validators.required),     
    }
  );

  // selectedFile:File | null = null; // Store the selected file
  // onFileSelected(event:any)
  // {
  //   const file = event.target.files[0];// Get the selected file
  //   if(file)
  //   {
  //     this.selectedFile=file;
  //   }
  // }


  // addProduct()
  // {
  //   if(this.addProductForm.valid)
  //     {
  //       const formData = new FormData(); //eppo  ee formData endaa nu veychaal, nammalk ee filers FormGroup int koodee send aakann pattulaaa, but formData() can pass any datas , so nammal ee formGroupine oru FormnData ilott covert cheythuu
  //       formData.append("nameOfProduct",this.addProductForm.value.nameOfProduct ?? "");
  //       formData.append("descriptionOfProduct",this.addProductForm.value.descriptionOfProduct ?? "");
  //       formData.append("priceOfProduct",this.addProductForm.value.priceOfProduct ?? "");
  //       formData.append("availableQuantity",this.addProductForm.value.availableQuantity ?? "");
  //       // formData.append("idOfCategory",this.addProductForm.value.idOfCategory ?? "");

  //       // Add category ID safely
  //       if (this.addProductForm.value.idOfCategory) {    
  //           // here need to change the category name to the correspoinding categorys ID (cuz its a foreign key connection with the category table) 
  //         formData.append("idOfCategory", this.categoryId);
  //       }

  //       if(this.selectedFile) // for carefully adding image
  //       {
  //         formData.append("product_image",this.selectedFile,this.selectedFile.name);

  //         console.log(this.selectedFile);
          
  //       }
            
        
        
  //       this.service.GetCategoryId(this.addProductForm.value.idOfCategory).subscribe({
  //         next:(res:any)=>
  //         {
  //           if(res.status)
  //           {
              
  //             formData.delete("idOfCategory");  // ✅ Remove any existing value
  //             formData.append("idOfCategory", res.data.toString());  // ✅ Ensure correct format
  //             console.log(formData.get("idOfCategory"));

              
  //             console.log(formData)

  //             console.log(formData);
  //                 this.service.AddProductService(formData).subscribe({
  //                   next:(res1:any)=>
  //                   {
  //                     if(res1.status)
  //                     {
  //                       alert(res1.message);
  //                       console.log(res1);                       
  //                       this.router.navigate(["admin/dashboard"]);
  //                     }
  //                     else
  //                     {
  //                       alert(res1.message);
  //                       console.log(res1); 
  //                     }
  //                   },
  //                   error:error=>
  //                   {
  //                     // alert(error.message);
  //                     console.log(error);
  //                   }
  //                 });
  //           }
  //           else
  //           {
  //             // alert(res.message);
  //             console.log(res);             
  //           }
  //         },
  //         error:error=>
  //         {
  //           // alert(error.message);
  //           console.log(error);
            
  //         }
  //       });
  //     }   
  //     else
  //     {
  //       alert("Invalid form format")
  //     }
  // }

  selectedFile :File | null = null;

  onFileSelected(event:any)
  {
    const file = event.target.files[0]; //here we are storing the image to variable file
    if(file)
    {
      this.selectedFile = file;  // we are storing that image into the variabke selected file;
    }
  }

  //for addProduct()
  addProduct()
  {
    if(this.addProductForm.valid)
    {
      const formData = new FormData();
      formData.append("nameOfProduct",this.addProductForm.value.nameOfProduct ?? ""); //added name
      formData.append("descriptionOfProduct",this.addProductForm.value.descriptionOfProduct ?? ""); //added description
      formData.append("priceOfProduct",this.addProductForm.value.priceOfProduct ?? ""); //added price
     formData.append("availableQuantity",this.addProductForm.value.availableQuantity ?? ""); //added Quantity

      if(this.selectedFile) //for safely handle the image
      {
        formData.append("product_image",this.selectedFile,this.selectedFile.name);  //added Image (but still we didnt added category)
      }

      this.service.GetCategoryId(this.addProductForm.value.idOfCategory).subscribe({
        next:(res:any)=>
        {
          if(res.status)
          {
            console.log("the category id is",res.data);
            formData.append("idOfCategory",res.data);       //Added category ID to the formData and now we send this form data
            
            this.service.AddProductService(formData).subscribe({
              next:(res1:any)=>
              {
                if(res.status)
                {
                  console.log(res);
                  alert(res.message)
                  this.router.navigate(["admin/"]);
                }
                else{
                  console.log(res);
                  alert(res.message)
                }
              },
              error:error=>
              {
                console.log(error);
                alert(error.message)
              }
            })
          }
          else
          {
            console.log(res);
            alert(res.message)
          }
        },
        error:error=>
        {
          console.log(error.message);
          alert(error.message);
        }
      });
    }
    else
    {
      alert("invalid format")
    }
  }

  //for resetForm()
  reset()
  {
    this.addProductForm.reset();
  }

  //for goBack
  goback()
  {
    this.router.navigate(["admin/dashboard"]);
  }

}
