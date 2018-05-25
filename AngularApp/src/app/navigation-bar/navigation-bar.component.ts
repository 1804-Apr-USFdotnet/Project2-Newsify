import { Component, OnInit, ViewChild } from '@angular/core';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { Article } from '../models/article';
import { ArticlesService } from '../articles.service';
import { ArticlesComponent } from '../articles/articles.component';

@Component({
  selector: 'app-navigation-bar',
  templateUrl: './navigation-bar.component.html',
  styleUrls: ['./navigation-bar.component.css']
})
export class NavigationBarComponent implements OnInit {

  constructor(private arSvc: ArticlesService) { }

  ngOnInit() {
  }
  @ViewChild(ArticlesComponent) art : ArticlesComponent;
  
  MyEvent(){
    //this.art.searchArticles();
    // this.arSvc.getArticles((response) => {
    // this.articles = response.articles;
    // });
  }
}

