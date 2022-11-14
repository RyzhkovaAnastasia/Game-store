import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Shipper } from 'src/app/classes/shipper';
import { ShipperService } from 'src/app/services/shipper.service';

@Component({
  selector: 'app-shippers',
  templateUrl: './shippers.component.html',
  styleUrls: ['./shippers.component.css']
})
export class ShippersComponent implements OnInit {

  public shippers: Shipper[] = [];

  constructor(
    private readonly _shipperService: ShipperService,
    private readonly _toastr: ToastrService) { }

  ngOnInit(): void {
    this.getAll();
  }

  private getAll(): void {
    this._shipperService.getShippers()
      .subscribe({
        next: responseBody => this.shippers = responseBody
      });
  }
}
