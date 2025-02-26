import { Component } from '@angular/core';
import { FormControl,FormGroup,Validators } from '@angular/forms';
import { AdminServiceService } from 'src/app/Services/admin-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-layout',
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.css']
})
export class AdminLayoutComponent {
  constructor(private adminService:AdminServiceService,private router:Router) {}

  adminData:any;
  ngOnInit()
  {
    // this.adminData=history.state.admin;  
    if(localStorage.getItem("adminData"))
    {
      this.adminData=JSON.parse(localStorage.getItem("adminData")!);
    }
    else
    {
      localStorage.clear;
      this.router.navigate([""]);
    }
  }

  searchUser:boolean=false;     //for enable the search bar for to search user
  searchCategory:boolean=false;  //for enable the search bar for to search product


  //FOR enable seacrh a user
  searchUserEnableButton()
  {
    this.searchCategory = false;
    this.searchUser = !this.searchUser; // Toggle behavior
    // Ensure the search form is reset when enabling search
  if (this.searchUser) {
    this.searchNameForm.reset();
  }
  }

    //FOR enable seacrh a product
  searchCategoryEnableButton()
  {
    this.searchUser=false;
    this.searchCategory=true;       
  }

  //for search if its a user or not
   searchNameForm= new FormGroup(
    {
      nameOfUser:new FormControl(""),
    })

    //for search a category
    SearchCategoryForm = new FormGroup(
      {
        categoryName : new FormControl(""),
      }
    )
  

  // resting the form
  resetSearchForm()
  {
    this.searchNameForm.reset();
  }
  //Search a single user or mutliple user by name ENDS here


}
