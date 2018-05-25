import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {Routes, RouterModule} from '@angular/router'
import { ArticlesComponent } from './articles/articles.component';

const appRoutes: Routes = [
  {path: "articles", component: ArticlesComponent},
  {path: "articles/:id/:input", component: ArticlesComponent},
  {path: '', redirectTo: '/articles', pathMatch: 'full' },
]

@NgModule({
  exports: [ RouterModule ],
  imports: [
    CommonModule, 
    RouterModule.forRoot(appRoutes)
  ],
  declarations: []
})
export class AppRoutingModule { }
