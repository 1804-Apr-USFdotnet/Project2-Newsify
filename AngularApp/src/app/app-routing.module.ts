import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {Routes, RouterModule} from '@angular/router'
import { ArticlesComponent } from './articles/articles.component';

const appRoutes: Routes = [
  {path: "articles", component: ArticlesComponent}
]

@NgModule({
  imports: [
    CommonModule, 
    RouterModule.forRoot(appRoutes)
  ],
  declarations: []
})
export class AppRoutingModule { }
