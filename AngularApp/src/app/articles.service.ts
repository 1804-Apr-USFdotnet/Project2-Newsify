import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ArticlesService {

  constructor(private httpClient: HttpClient) { }

  getArticles(onSuccess){
    var url = "https://newsapi.org/v2/top-headlines?country=us&apiKey=33b909af4e294034ad07bd3546790502";
    var req = this.httpClient.get(url);
    var promise = req.toPromise();
    promise.then(
      onSuccess,
      (reason) => console.log(reason)
    )
  }
  getArticlesByTitle(type :string, input:string, onSuccess){
    console.log(type + " " + input);
    if (type === "title"){
       var url = "https://newsapi.org/v2/everything?q=" + input + "&apiKey=33b909af4e294034ad07bd3546790502";
    }else if(type === "source"){
       var url = "https://newsapi.org/v2/sources?category=" + input + "&apiKey=33b909af4e294034ad07bd3546790502";
    }else if(type === "lang"){
       var url = "https://newsapi.org/v2/everything?language=" + input + "&apiKey=33b909af4e294034ad07bd3546790502";
    }else{
      console.log("not a valid type");
    }
    var req = this.httpClient.get(url);
    var promise = req.toPromise();
    promise.then(
      onSuccess,
      (reason) => console.log(reason)
    )
  }
}