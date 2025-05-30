import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = () => {
  const router = inject(Router);
  const usuario = localStorage.getItem('usuario');

  if (!usuario) {
    router.navigate(['/']);
    return false;
  }

  return true;
};
