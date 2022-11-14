import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  
})
export class AppComponent {

  title = 'online-game-store-front';

  constructor(private translate: TranslateService) {
    if(localStorage.getItem('language')){
      translate.setDefaultLang(localStorage.getItem('language') ?? 'en');
      translate.use(localStorage.getItem('language') ?? 'en');
    }else {
       translate.setDefaultLang('en');
       translate.use('en');
       localStorage.setItem("language","en");
  }
    
  }
}
