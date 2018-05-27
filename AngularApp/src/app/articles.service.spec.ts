import { TestBed, inject } from '@angular/core/testing';
import { HttpClientModule } from '@angular/common/http';
import { ArticlesService } from './articles.service';
import { Article } from './models/article';


describe('ArticlesService', () => {
  let service = ArticlesService;
  beforeEach(() => {
    //service = new ArticlesService();
    TestBed.configureTestingModule({
      providers: [ArticlesService],
      imports: [HttpClientModule]
    });
  });


  it('should be created', inject([ArticlesService], (service: ArticlesService) => {
    expect(service).toBeTruthy();
  }));

  it('should return 20', inject([ArticlesService], (service: ArticlesService) => {
    service.getArticles((response) => {
      expect(response.articles.length).toBe(20);
    },
      fail);
  }));

  it('should return 20', inject([ArticlesService], (service: ArticlesService) => {
    service.getArticlesApi("Title", "Tesla", (response) => {
      expect(response.length).toBe(4);
    });
  }));
  it('should return 20', inject([ArticlesService], (service: ArticlesService) => {
    service.getArticlesApi("Source", "CNN", (response) => {
      expect(response.length).toBe(2);
    });
  }));
  it('should return 20', inject([ArticlesService], (service: ArticlesService) => {
    service.getArticlesApi("Topic", "Microsoft", (response) => {
      expect(response.length).toBe(100);
    });
  }));
  it('should return 20', inject([ArticlesService], (service: ArticlesService) => {
    service.getArticlesApi("Country", "CA", (response) => {
      expect(response.length).toBe(1);
    });
  }));
  it('should return 20', inject([ArticlesService], (service: ArticlesService) => {
    service.getArticlesApi("Language", "EN", (response) => {
      expect(response.length).toBe(100);
    });
  }));
});