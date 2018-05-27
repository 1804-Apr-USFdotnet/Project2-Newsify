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
    this.arSvc.getArticles((response) => {
      console.log(response);
      this.articles = response.articles;
    });
  }
  searchArticlesApi(type, input) {
    this.arSvc.getArticlesApi(type, input, (response) => {
      this.articles = response;
      console.log(response);
    });
  }

}
