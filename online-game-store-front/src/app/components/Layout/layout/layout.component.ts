import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { faGear, faKey, faShoppingCart } from '@fortawesome/free-solid-svg-icons';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/classes/auth/user';
import { Role } from 'src/app/enums/role';
import { GameService } from 'src/app/services/game.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class LayoutComponent {

  public user: User;
  gameNumber: number = 0;
  faBasket = faShoppingCart;
  faDoor = faKey;
  faSettings = faGear;
  isSignIn: boolean;
  isInRole: boolean;
  isInAdminRole: boolean;

  constructor(
    public modalService: BsModalService,
    private readonly _gameService: GameService,
    private readonly _userService: UserService,
    private readonly _router: Router,
    private readonly _toastr: ToastrService)
  {
    this._gameService.getGameNumber().subscribe((responseBody)=> {this.gameNumber = responseBody});
    this.isSignIn = this._userService.isAuthenticated;
    this.user = _userService.getUser;
    this.isInRole = this.user.role === Role.Admin || this.user.role === Role.Manager;
    this.isInAdminRole = this.user.role === Role.Admin;
  }

  sign(){
    if(this._userService.isAuthenticated){
      this._userService.signOut();
      this._toastr.error("You left account", "Sign out");
    }
      this._router.navigate(["/auth"]);
    this.isSignIn  = this._userService.isAuthenticated;
  }
}
