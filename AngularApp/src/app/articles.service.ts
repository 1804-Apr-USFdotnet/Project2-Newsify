import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ArticlesService {

  constructor(private httpClient: HttpClient) { }

  getArticles(onSuccess) {
    var url = "https://newsapi.org/v2/top-headlines?country=us&apiKey=33b909af4e294034ad07bd3546790502";
    var req = this.httpClient.get(url);
    var promise = req.toPromise();
    promise.then(
      onSuccess,
      (reason) => console.log(reason)
    )
  }
  getArticlesApi(type, input, onSuccess) {
    var host = "http://localhost:3272/"
    if (type === "Title") {
      var url = host + "api/Data/Title";
    } else if (type === "Source") {
      var url = host + "api/Data/Source";
    } else if (type === "Language") {
      var url = host + "api/Data/Language";
    } else if (type === "Country") {
      var url = host + "api/Data/Country";
    } else if (type === "Topic") {
      var url = host + "api/Data/Topic";
    } else if (type === "Date") {
      var url = host + "api/Data/Date";
    } else {
      console.log("not a valid type");
    }
    var req = this.httpClient.post(url, type + "=" + input,
      { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } });
    console.log(req)
    var promise = req.toPromise();
    promise.then(
      onSuccess,
      (reason) => console.log(reason)
    )
  }

  getArticlesByTitle(type: string, input: string, onSuccess) {
    console.log(type + " " + input);
    if (type === "title") {
      var url = "https://newsapi.org/v2/everything?q=" + input + "&apiKey=33b909af4e294034ad07bd3546790502";
    } else if (type === "source") {
      var url = "https://newsapi.org/v2/sources?category=" + input + "&apiKey=33b909af4e294034ad07bd3546790502";
    } else if (type === "lang") {
      var url = "https://newsapi.org/v2/everything?language=" + input + "&apiKey=33b909af4e294034ad07bd3546790502";
    } else {
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