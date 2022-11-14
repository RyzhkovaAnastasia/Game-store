import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommentsComponent } from './components/Comment/comments/comments.component';
import { GameCreatorComponent } from './components/Game/game-creator/game-creator.component';
import { GameEditorComponent } from './components/Game/game-editor/game-editor.component';
import { GameComponent } from './components/Game/game/game.component';
import { GamesComponent } from './components/Game/games/games.component';
import { GenreCreatorComponent } from './components/Genre/genre-creator/genre-creator.component';
import { GenreEditorComponent } from './components/Genre/genre-editor/genre-editor.component';
import { GenresComponent } from './components/Genre/genres/genres.component';
import { BasketComponent } from './components/Order/basket/basket.component';
import { DeliveryComponent } from './components/Order/delivery/delivery.component';
import { IboxPaymentComponent } from './components/Order/ibox-payment/ibox-payment.component';
import { OrderComponent } from './components/Order/order/order.component';
import { OrdersComponent } from './components/Order/orders/orders.component';
import { VisaPaymentComponent } from './components/Order/visa-payment/visa-payment.component';
import { PlatformTypeCreatorComponent } from './components/PlatformType/platform-type-creator/platform-type-creator.component';
import { PlatformTypeEditorComponent } from './components/PlatformType/platform-type-editor/platform-type-editor.component';
import { PlatformTypesComponent } from './components/PlatformType/platform-types/platform-types.component';
import { PublisherCreatorComponent } from './components/Publisher/publisher-creator/publisher-creator.component';
import { PublisherEditorComponent } from './components/Publisher/publisher-editor/publisher-editor.component';
import { PublisherComponent } from './components/Publisher/publisher/publisher.component';
import { PublishersComponent } from './components/Publisher/publishers/publishers.component';
import { ShippersComponent } from './components/Shipper/shippers/shippers.component';
import { SignInComponent } from './components/Auth/sign-in/sign-in.component';
import { SignUpComponent } from './components/Auth/sign-up/sign-up.component';
import { UserCommentBanComponent } from './components/User/user-comment-ban/user-comment-ban.component';
import { AuthLayoutComponent } from './components/Auth/auth-layout/auth-layout.component';
import { AppComponent } from './app.component';
import { LayoutComponent } from './components/Layout/layout/layout.component';
import { OrdersHistoryComponent } from './components/Order/orders-history/orders-history.component';
import { UserSettingsComponent } from './components/User/user-settings/user-settings.component';
import { UsersComponent } from './components/User/users/users.component';
import { UserEditComponent } from './components/User/user-edit/user-edit.component';
import { OrderEditComponent } from './components/Order/order-edit/order-edit.component';
import { UnauthorizedErrorComponent } from './http-errors/unauthorized-error/unauthorized-error.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children:
      [
        { path: 'shippers', component: ShippersComponent },

        { path: 'publishers', component: PublishersComponent },
        { path: 'publishers/update/:companyName', component: PublisherEditorComponent },
        { path: 'publishers/new', component: PublisherCreatorComponent },
        { path: 'publishers/:companyName', component: PublisherComponent },

        { path: 'genres', component: GenresComponent },
        { path: 'genres/update/:id', component: GenreEditorComponent },
        { path: 'genres/new', component: GenreCreatorComponent },

        { path: 'platforms', component: PlatformTypesComponent },
        { path: 'platforms/update/:id', component: PlatformTypeEditorComponent },
        { path: 'platforms/new', component: PlatformTypeCreatorComponent },

        { path: 'games' || 'games/filter/:filter', component: GamesComponent },
        { path: 'games/update/:gamekey', component: GameEditorComponent },
        { path: 'games/new', component: GameCreatorComponent },
        { path: 'games/:gamekey', component: GameComponent },

        { path: 'games/:gamekey/comments', component: CommentsComponent },
        { path: 'commentban/duration', component: UserCommentBanComponent },

        { path: 'order', component: OrderComponent },
        { path: 'orders', component: OrdersComponent },
        { path: 'orders/update/:id', component: OrderEditComponent },
        { path: 'orders/history', component: OrdersHistoryComponent },
        { path: 'basket', component: BasketComponent },
        { path: 'order/delivery', component: DeliveryComponent },
        { path: 'order/payment/visa', component: VisaPaymentComponent },
        { path: 'order/payment/ibox', component: IboxPaymentComponent },

        { path: 'users', component: UsersComponent },
        { path: 'users/update/:username', component: UserEditComponent },
        { path: 'settings', component: UserSettingsComponent },

        { path: '', redirectTo: '/games', pathMatch: 'full' },

        { path: 'unauthorized-error', component: UnauthorizedErrorComponent },
      ]
  },
  {
    path: 'auth',
    component: AuthLayoutComponent,
    children: [
      {
        path: '',
        redirectTo: 'sign-in',
        pathMatch: 'full'
      },
      {
        path: 'sign-in',
        component: SignInComponent
      },
      {
        path: 'sign-up',
        component: SignUpComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes), RouterModule.forRoot(routes, { anchorScrolling: 'enabled' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
