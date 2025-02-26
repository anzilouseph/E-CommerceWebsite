import { Component } from '@angular/core';
import { AdminServiceService } from 'src/app/Services/admin-service.service';
import { FormControl,FormGroup,Validators } from '@angular/forms';
import { Router } from '@angular/router';
@Component({
  selector: 'app-add-user-comp',
  templateUrl: './add-user-comp.component.html',
  styleUrls: ['./add-user-comp.component.css']
})
export class AddUserCompComponent {

  constructor(private adminService:AdminServiceService,private router:Router) {}

  //for AddUser when the Add User Button enable it works (IMP)

  AddUserByAdminForm = new FormGroup(
    {
      nameOfUser:new FormControl("",[Validators.required,Validators.pattern("^[a-zA-Z ]+$")]),
      phoneOfUser: new FormControl("",[Validators.required,Validators.pattern("^[0-9]{10}$")]),
      emailOfUser:new FormControl("",[Validators.required, Validators.pattern("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")]),
      passwordOfUser:new FormControl("",Validators.required),
      roleOfUser : new FormControl("",[Validators.required,Validators.pattern("^(Admin|User)$")]),
    });


AddUserByAdminFormData:any;
errorMessage:any;


AddUserByAdminComponentFn()
{
  if(this.AddUserByAdminForm.valid)
  {
    this.AddUserByAdminFormData=this.AddUserByAdminForm.value;
    this.adminService.AddUserByAdminServiceFn(this.AddUserByAdminFormData).subscribe({
      next:(res:any)=>
      {
        if(res.status)
        {
          alert(res.message);
          console.log(res);
          this.router.navigate(['/admin/dashboard']);
        }
        else
        {
          alert(res.message);
          console.log(res);
          this.router.navigate(['/admin/dashboard']);
        }
      },
      error:error=>
      {
        alert(error.message);
        console.log(error);  
        this.router.navigate(['/admin/dashboard']);
      }
    });  
  }
  else
  {
    alert("Invalid format");
    this.AddUserByAdminForm.reset();
  }
}

resetForm() //for reseting the form
{
  this.AddUserByAdminForm.reset();
}
//Add user things Ends Here

}
