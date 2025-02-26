import { Component } from '@angular/core';
import { UserServiceService } from '../Services/user-service.service';
import { FormControl,FormGroup,Validators } from '@angular/forms';
import { Router } from '@angular/router';
@Component({
  selector: 'app-registration-comp',
  templateUrl: './registration-comp.component.html',
  styleUrls: ['./registration-comp.component.css']
})
export class RegistrationCompComponent {
  constructor(private userService:UserServiceService,private router:Router){}

  registrationForm = new FormGroup(
    {
      nameOfUser:new FormControl("",[Validators.required,Validators.pattern("^[a-zA-Z ]+$")]),
      phoneOfUser: new FormControl("",[Validators.required,Validators.pattern("^[0-9]{10}$")]),
      emailOfUser:new FormControl("",[Validators.required, Validators.pattern("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")]),
      passwordOfUser:new FormControl("",Validators.required)

    }
  );

  registrationFormData:any;
  data:any;
  errorMessage:any;

  UserRegistrationFn()
  {
    if(this.registrationForm.valid)
    {
      this.registrationFormData=this.registrationForm.value;
      this.userService.UserRegistrationServiceFn(this.registrationFormData).subscribe({
        next:(res:any)=>
        {
          if(res.status)
          {
            alert(res.message);
            this.router.navigate([""]);
            console.log(res);
            
          }
          else
          {
            alert(res.message);
            console.log(res);           
          }
        },
        error:error=>
        {
          this.errorMessage=error.message;
        }
      });
    }
    else
    {
      alert("Invalid form format");
    }
  }

}
