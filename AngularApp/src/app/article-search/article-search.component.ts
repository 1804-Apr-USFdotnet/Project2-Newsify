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

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private arSvc: ArticlesService) { }

  //public type = this.route.snapshot.paramMap.get('id');
  //public input = this.route.snapshot.paramMap.get('input')


  ngOnInit() {
    this.route.params.subscribe(params => {
      let type = params['id']
      let input = params['input'];
      this.searchArticlesApi(type, input);
    })
  }
  searchArticlesApi(type, input) {
    this.arSvc.getArticlesApi(type, input, (response) => {
      this.articles = response;
      console.log(response);
    });
  }

}
