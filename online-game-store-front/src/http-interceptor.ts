import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Observable, catchError, throwError } from 'rxjs';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private router: Router,
        private _toastrService: ToastrService,
        private _translator: TranslateService) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        
        return next.handle(request)
        .pipe(catchError(err => {
            if (err.status === 401 || err.status === 403) {
                 this.router.navigate(['/unauthorized-error']);
            }
            const error = err.error || err.statusText;
            switch(this._translator.getDefaultLang()){
                case 'uk':
                    this._toastrService.error(error?.message, "Помилка"); 
                    break;
                case 'ru':
                    this._toastrService.error(error?.message, "Ошибка");
                    break;
                default:
                    this._toastrService.error(error?.message, "Error");
                    break;
            }
            return throwError(error);
        }))
    }
}