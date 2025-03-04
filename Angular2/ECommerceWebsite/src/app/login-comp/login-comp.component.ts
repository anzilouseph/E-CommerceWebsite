import { Component } from '@angular/core';
import { AuthenticationServiceService } from '../Services/authentication-service.service';
import { UserServiceService } from '../Services/user-service.service';
import { FormControl,FormGroup,Validators } from '@angular/forms';
import { Router } from '@angular/router';
@Component({
  selector: 'app-login-comp',
  templateUrl: './login-comp.component.html',
  styleUrls: ['./login-comp.component.css']
})
export class LoginCompComponent {

  constructor(private authService:AuthenticationServiceService,private userService:UserServiceService,private router:Router){}

  loginForm = new FormGroup(
    {
      emailOfUser: new FormControl("",[Validators.required,Validators.pattern("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")]),
      passwordOfUser : new FormControl("",Validators.required),
    }
  );

  loginFormData:any;
  data:any;
  errorMessage:any;


  loginCompFn()
  {
    if(this.loginForm.valid)
    {
      this.loginFormData = this.loginForm.value;
      this.authService.loginSericeFn(this.loginFormData).subscribe({
        next:(res:any)=>
        {
          if(res.status)
          {
            console.log(res);
            localStorage.setItem("accessToken",res.data);
            //here we are calling the getOwn profile to checkn wheather the logined user is an Admin or a user
            this.userService.GetOwnProfileServiceFn().subscribe({
              next:(res1:any)=>
              {
                if(res1.status)
                {
                  
                  //checking admin or not
                  if(res1.data.roleOfUser === "Admin")
                  {
                    localStorage.setItem("adminData",JSON.stringify(res1.data));
                    this.router.navigate(["admin"]) //here if its a admin it wil navigate to the admin dashboard and the data of the admin is given to that page also
                  }
                  //if he is not an Admin then he must be the user
                  else
                  {
                    localStorage.setItem("userData",JSON.stringify(res1.data));
                    this.router.navigate(['UserLayout'])  //here if its a user it wil navigate to the user dashboard and the data of the user is given to that page also
                  }
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
                  console.log(error);
              }
            });
            
          }
          else
          {
            alert(res.message);
          }
        },
        error:error=>
        {
          alert(error.message);
        }
      });
    }
    else
      {
        alert("Invalid form format");
      }
  }

  resetForm()
  {
    this.loginForm.reset();
  }

}
