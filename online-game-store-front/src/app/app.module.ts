import { CommonModule } from "@angular/common";
import { TreeModule } from '@circlon/angular-tree-component'
import { ToastrModule } from 'ngx-toastr'
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome'
import {BrowserAnimationsModule} from '@angular/platform-browser/animations'
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgMultiSelectDropDownModule } from "ng-multiselect-dropdown";
import { ModalModule } from "ngx-bootstrap/modal";
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { CommentCreatorComponent } from "./components/Comment/comment-creator/comment-creator.component";
import { CommentEditorComponent } from "./components/Comment/comment-editor/comment-editor.component";
import { CommentsComponent } from "./components/Comment/comments/comments.component";
import { GameCreatorComponent } from "./components/Game/game-creator/game-creator.component";
import { GameEditorComponent } from "./components/Game/game-editor/game-editor.component";
import { GameComponent } from "./components/Game/game/game.component";
import { GamesFilterComponent } from "./components/Game/games-filter/games-filter.component";
import { GamesPaginationComponent } from "./components/Game/games-pagination/games-pagination.component";
import { GamesSortComponent } from "./components/Game/games-sort/games-sort.component";
import { GamesComponent } from "./components/Game/games/games.component";
import { GenreCreatorComponent } from "./components/Genre/genre-creator/genre-creator.component";
import { GenreEditorComponent } from "./components/Genre/genre-editor/genre-editor.component";
import { GenresComponent } from "./components/Genre/genres/genres.component";
import { BasketComponent } from "./components/Order/basket/basket.component";
import { IboxPaymentComponent } from "./components/Order/ibox-payment/ibox-payment.component";
import { OrderComponent } from "./components/Order/order/order.component";
import { OrdersComponent } from "./components/Order/orders/orders.component";
import { PaymentMethodsComponent } from "./components/Order/payment-methods/payment-methods.component";
import { VisaPaymentComponent } from "./components/Order/visa-payment/visa-payment.component";
import { PlatformTypeCreatorComponent } from "./components/PlatformType/platform-type-creator/platform-type-creator.component";
import { PlatformTypeEditorComponent } from "./components/PlatformType/platform-type-editor/platform-type-editor.component";
import { PlatformTypesComponent } from "./components/PlatformType/platform-types/platform-types.component";
import { PublisherCreatorComponent } from "./components/Publisher/publisher-creator/publisher-creator.component";
import { PublisherEditorComponent } from "./components/Publisher/publisher-editor/publisher-editor.component";
import { PublisherComponent } from "./components/Publisher/publisher/publisher.component";
import { PublishersComponent } from "./components/Publisher/publishers/publishers.component";
import { DeleteModalComponent } from "./components/Shared/delete-modal/delete-modal.component";
import { ShippersComponent } from "./components/Shipper/shippers/shippers.component";
import { UserCommentBanComponent } from "./components/User/user-comment-ban/user-comment-ban.component";
import { GameCacheService } from "./services/game-cache.service";
import { NgxDaterangepickerMd } from "ngx-daterangepicker-material";
import { DeliveryComponent } from './components/Order/delivery/delivery.component';
import { SignUpComponent } from './components/Auth/sign-up/sign-up.component';
import { SignInComponent } from './components/Auth/sign-in/sign-in.component';
import { AuthLayoutComponent } from "./components/Auth/auth-layout/auth-layout.component";
import { LayoutComponent } from './components/Layout/layout/layout.component';
import { environment } from "src/environments/environment";
import { OrdersHistoryComponent } from "./components/Order/orders-history/orders-history.component";
import { UserSettingsComponent } from './components/User/user-settings/user-settings.component';
import { UsersComponent } from './components/User/users/users.component';
import { UserEditComponent } from './components/User/user-edit/user-edit.component';
import { OrderEditComponent } from './components/Order/order-edit/order-edit.component';
import { UnauthorizedErrorComponent } from './http-errors/unauthorized-error/unauthorized-error.component';
import { ErrorInterceptor } from "src/http-interceptor";
import { JwtModule } from "@auth0/angular-jwt";
import { TranslateLoader, TranslateModule } from "@ngx-translate/core";
import { TranslateHttpLoader } from "@ngx-translate/http-loader";

function tokenGetter(): string | null {
  return localStorage.getItem('ACCESS_TOKEN_KEY');
}


@NgModule({
  declarations: [
    AppComponent,
    AuthLayoutComponent,
    GamesComponent,
    GameEditorComponent,
    GameCreatorComponent,
    GenreCreatorComponent,
    GenreEditorComponent,
    GenresComponent,
    PlatformTypeCreatorComponent,
    PlatformTypeEditorComponent,
    PlatformTypesComponent,
    PublishersComponent,
    PublisherCreatorComponent,
    PublisherEditorComponent,
    BasketComponent,
    OrderComponent,
    PaymentMethodsComponent,
    VisaPaymentComponent,
    IboxPaymentComponent,
    CommentCreatorComponent,
    CommentEditorComponent,
    CommentsComponent,
    UserCommentBanComponent,
    GameComponent,
    PublisherComponent,
    DeleteModalComponent,
    GamesFilterComponent,
    GamesSortComponent,
    GamesPaginationComponent,
    ShippersComponent,
    OrdersComponent,
    OrdersHistoryComponent,
    DeliveryComponent,
    SignUpComponent,
    SignInComponent,
    LayoutComponent,
    UserSettingsComponent,
    UsersComponent,
    UserEditComponent,
    OrderEditComponent,
    UnauthorizedErrorComponent,
  ],
  imports: [
    CommonModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    TreeModule,
    ModalModule.forRoot(),
    FontAwesomeModule,
    FormsModule,
    NgMultiSelectDropDownModule.forRoot(),
    ReactiveFormsModule,
    NgxDaterangepickerMd.forRoot(),
    JwtModule.forRoot({
      config: {
        tokenGetter,
        allowedDomains: environment.allowedApiDomainsAuth,
      }
    }),
    TranslateModule.forRoot({
        loader: {
            provide: TranslateLoader,
            useFactory: HttpLoaderFactory,
            deps: [HttpClient]
        }
    })
  ],
  providers: [
    GameCacheService,
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
  schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class AppModule { }

export function HttpLoaderFactory(http: HttpClient): TranslateHttpLoader {
  return new TranslateHttpLoader(http);
}
