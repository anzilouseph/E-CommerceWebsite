import { Component } from '@angular/core';
import { AdminServiceService } from 'src/app/Services/admin-service.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-user-mangement-comp',
  templateUrl: './user-mangement-comp.component.html',
  styleUrls: ['./user-mangement-comp.component.css']
})
export class UserMangementCompComponent {

  constructor(private adminService:AdminServiceService,private route:Router) {}


  //GetAll Users functinalitys starts here (Imp) MUST READ COMMENTS , it mention where this functionalities ends
  
  ngOnInit()
  {
    this.getAllUsersByAdmin();
  }

  getAllData:any;
  errorMessage:any;

  getAllUsersByAdmin()    //only admin can access this
  {
     this.adminService.GetAllAdminServiceFn().subscribe({
      next:(res:any)=>
      {
        if(res.status)
        {
         this.getAllData = res.data;
         console.log(res);
        }
        else
        {
          this.errorMessage=res.message;
          console.log(res);
          
        }
      },
      error:error=>
      {
        this.errorMessage=error.message;
      }
    });
  }
//GetAll Users functinalitys Ends here


//for delete a user (only admin can access this) starts here
deleteUser(id:any)
{
  this.adminService.DeleteUser(id).subscribe({
    next:(res:any)=>
    {
      if(res.status)
      {
       
        alert(res.message);
        this.getAllUsersByAdmin();
        console.log(res);       
      }
      else{
        alert(res.message);
        this.getAllUsersByAdmin();
        console.log(res);
      }
    },
    error:error=>
    {
      alert(error.message);
    }
  })
}
//for delete a user (only admin can access this) ENDS HERE

}
