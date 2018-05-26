import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
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
  
  constructor(
    private route: ActivatedRoute,
    private location: Location, 
    private arSvc: ArticlesService ) { }

    public type = this.route.snapshot.paramMap.get('id');
    public input = this.route.snapshot.paramMap.get('input')


  ngOnInit() {
    this.searchArticles(this.type, this.input);
    this.arSvc.getArticles((response) => {
    this.articles = response.articles;
    });
  }
  searchArticles(type, input) {
    this.arSvc.getArticlesByTitle(type, input, (response) => {
      this.articles = response.articles;
    });
  }

}
