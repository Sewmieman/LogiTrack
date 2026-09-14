import { Routes } from '@angular/router';

import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
import { Layout } from './layout/layout';

import { Dashboard } from './pages/dashboard/dashboard';
import { Customers } from './pages/customers/customers';
import { Drivers } from './pages/drivers/drivers';
import { Deliveries } from './pages/deliveries/deliveries';
import { Payments } from './pages/payments/payments';
import { Vehicles } from './pages/vehicles/vehicles';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },

  {
    path: 'login',
    component: Login
  },

  {
    path: 'register',
    component: Register
  },

  {
    path: '',
    component: Layout,
    children: [
      {
        path: 'dashboard',
        component: Dashboard
      },
      {
        path: 'customers',
        component: Customers
      },
      {
        path: 'drivers',
        component: Drivers
      },
      {
        path: 'deliveries',
        component: Deliveries
      },
      {
        path: 'payments',
        component: Payments
      },
      {
        path: 'vehicles',
        component: Vehicles
      }
    ]
  },

  {
    path: '**',
    redirectTo: 'login'
  }
];