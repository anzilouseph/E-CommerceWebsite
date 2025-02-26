import { Component } from '@angular/core';
import { AdminServiceService } from 'src/app/Services/admin-service.service';
import { FormControl,FormGroup,Validators } from '@angular/forms';
import { AdminLayoutComponent } from 'src/app/Admin/AdminLayout/admin-layout/admin-layout.component';
@Component({
  selector: 'app-search-user-comp',
  templateUrl: './search-user-comp.component.html',
  styleUrls: ['./search-user-comp.component.css']
})
export class SearchUserCompComponent {
  constructor(private adminService:AdminServiceService,private adminLayoutComp:AdminLayoutComponent) {}

  errorMessage:any;
  data:any
  searchName:any;


  // searchNameForm= new FormGroup(  // ee form nammal admin layout il ezhuithi athine dependenxy injection cheyth veychitt indd
  //   {
  //     nameOfUser:new FormControl(""),
  //   }
  // )   

  ngOnInit()
  {
    this.getUserbyName()
  }


  getUserbyName()
  {
    this.searchName=this.adminLayoutComp.searchNameForm.get('nameOfUser')?.value; // ee form nammal admin layout il ezhuithi athine dependenxy injection cheyth veychitt indd, athil ninnum aan nammal ee name edukkane
    this.adminService.GetByNameAdminServiceFn(this.searchName).subscribe({
      next:(res:any)=>
      {
        if(res.status)
        {
          this.data=res.data;
          this.errorMessage=null; //multiple times il nammal search cheyumbo ooro thavaneem puthiya data veenam store aavan
          console.log(res);
          this.adminLayoutComp.resetSearchForm();
        }
        else
        {
          this.errorMessage = res.message;
          console.log(res);
          this.adminLayoutComp.resetSearchForm();
        }
      },
      error:error=>
      {
        this.errorMessage = error.message;
        this.data = null; //multiple times il nammal search cheyumbo ooro thavaneem puthiya data veenam store aavan
        this.adminLayoutComp.resetSearchForm();
        console.log('Error occurred:', error);
        
      }
    })
  }
  //Search a single user or mutliple user by name STARTS here

}
