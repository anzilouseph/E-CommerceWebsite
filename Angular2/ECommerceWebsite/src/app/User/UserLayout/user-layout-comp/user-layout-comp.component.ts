import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GetCategoryModel } from 'src/app/Models/GetCategoryModel';
import { AdminServiceService } from 'src/app/Services/admin-service.service';
import { CommonServicesService } from 'src/app/Services/common-services.service';

@Component({
  selector: 'app-user-layout-comp',
  templateUrl: './user-layout-comp.component.html',
  styleUrls: ['./user-layout-comp.component.css']
})
export class UserLayoutCompComponent {

constructor(private router:Router,private Aservice:AdminServiceService,private commonService:CommonServicesService){}
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
                                                  })
  }
  
}
