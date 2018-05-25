import { Component, OnInit } from '@angular/core';
import { Article } from '../models/article';
import { ArticlesService } from '../articles.service';

@Component({
  selector: 'app-articles',
  templateUrl: './articles.component.html',
  styleUrls: ['./articles.component.css']
})
export class ArticlesComponent implements OnInit {

  articles: Article[]=[
    
  ]

  //searchText: string;
  
  constructor(private arSvc: ArticlesService ) { }

  ngOnInit() {
    //this.searchArticles();
    this.arSvc.getArticles((response) => {
    this.articles = response.articles;
    });
  }
  // searchArticles() {
  //   this.arSvc.getArticles((response) => {
  //     this.articles = response.articles;
  //   });
  // }

}
