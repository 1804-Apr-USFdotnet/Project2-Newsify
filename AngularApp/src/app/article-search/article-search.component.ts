import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Article } from '../models/article';
import { ArticlesService } from '../articles.service';

@Component({
  selector: 'app-article-search',
  templateUrl: './article-search.component.html',
  styleUrls: ['./article-search.component.css']
})
export class ArticleSearchComponent implements OnInit {

  articles: Article[] = [

  ]

  //searchText: string;

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private arSvc: ArticlesService) { }

  public type = this.route.snapshot.paramMap.get('id');
  public input = this.route.snapshot.paramMap.get('input')


  ngOnInit() {
    // this.arSvc.getArticles((response) => {
    //   console.log(response);
    // this.articles = response.articles;
    // });
    this.searchArticlesApi(this.type, this.input);
  }
  searchArticlesApi(type, input) {
    this.arSvc.getArticlesApi(type, input, (response) => {
      this.articles = response;
      console.log(response);
    });
  }
  // searchArticles(type, input) {
  //   this.arSvc.getArticlesByTitle(type, input, (response) => {
  //     this.articles = response.articles;
  //   });
  // }

}
