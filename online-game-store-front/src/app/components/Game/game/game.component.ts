import { HttpResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import * as saveAs from 'file-saver';
import { Guid } from 'guid-typescript';
import { ToastrService } from 'ngx-toastr';
import { Game } from 'src/app/classes/game';
import { OrderDetail } from 'src/app/classes/orderDetail';
import { GameService } from 'src/app/services/game.service';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent {

  public game: Game;
  public blob: Blob;

  constructor(
    private readonly _gameService: GameService,
    private readonly _orderService: OrderService,
    private readonly _toastr: ToastrService,
    private readonly _route: ActivatedRoute,
    private readonly _router: Router
  ) {
    this.game =  new Game();
    this.blob = new Blob();

    let gamekey = this._route.snapshot.paramMap.get('gamekey') ?? '';
    this.getGame(gamekey);
    this._gameService.editGameViewNumber(gamekey).subscribe();
  }

  public getGame(gamekey: string): void {
    this._gameService.getGame(gamekey)
      .subscribe({
        next: responseBody => {
          this.game = responseBody;
        }
      });
  }

  public openGameComments(): void {
    this._router.navigate([`games`, this.game.key, `comments`]);
  }

  public addGameToBasket(): void {
    let orderDetail = new OrderDetail(Guid.EMPTY, this.game.id, null, this.game.price, 1, this.game.discount, Guid.EMPTY, null);
    this._orderService.addItemToOrder(orderDetail).subscribe(() => this._router.navigate(['basket']));
  }

  public downloadGame(): void {
    this._gameService.downloadGame(this.game.key).subscribe(this.downloadFileFromHttp);
  }

  private downloadFileFromHttp(httpResponse: HttpResponse<Blob>): void {
    const uriEncodedFileName = httpResponse?.headers?.get('content-disposition')?.match(/filename\*=UTF-8''(.*?)$/);
    if (uriEncodedFileName != null) {
      const fileName = decodeURI(uriEncodedFileName[1]);
      if (httpResponse.body != null) {
        const file = new File([httpResponse.body], fileName, { type: httpResponse.body.type });
        saveAs(file);
      }
    }
  }
  
  public getWithDiscount(price: number, discount: number): string {
    return (price - (price * discount / 100)).toFixed(2);
  }
}