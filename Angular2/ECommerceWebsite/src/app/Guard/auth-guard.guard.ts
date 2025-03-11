import { CanActivateFn } from '@angular/router';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { Injectable } from '@angular/core';
export const authGuardGuard: CanActivateFn = (route, state) => {
  
  const router = new Router();

  const roleNeeded = route.data['role'];

  if(roleNeeded === 'Admin'  && localStorage.getItem('adminData'))
  {
    return true; //Admin is logged in
  }

  if(roleNeeded==='User' && localStorage.getItem('userData'))
  {
    return true;
  }

  localStorage.clear();
  router.navigate(['']);
  return false;
};
