import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { UserComponent } from './user/user.component';
import { AuthGuard } from './_service/auth-guard.service';
import { AdduserComponent } from './adduser/adduser.component';
import { LoginlogComponent } from './loginlog/loginlog.component';
const routes: Routes = [
  {path:'', component:LoginComponent},
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard]},
  {path:'login', component:LoginComponent},
  { path: 'user', component: UserComponent, canActivate: [AuthGuard] },
  { path: 'adduser', component: AdduserComponent, canActivate: [AuthGuard] },
  /*{ path: 'edituser/:id', component: AdduserComponent, canActivate: [AuthGuard] },*/
  { path: 'loginlog', component: LoginlogComponent, canActivate: [AuthGuard] },
  {path:'**', component:LoginComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
