import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-payment-result',
  templateUrl: './payment-result.component.html'
})
export class PaymentResultComponent implements OnInit {
  private baseUrl: string;
  private http: HttpClient;
  private route: ActivatedRoute;

  model: PaymentVerifyResultViewModel;

  constructor(http: HttpClient, route: ActivatedRoute, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.http = http;
    this.route = route;
  }

  ngOnInit(): void {
    this.model = {} as PaymentVerifyResultViewModel;

    const orderId = this.route.snapshot.paramMap.get('id');
    
    this.http.get<PaymentVerifyResultViewModel>(this.baseUrl + `order/${orderId}`).subscribe(result => {
      this.model = result;
    }, error => console.error(error));
  }
}

interface PaymentVerifyResultViewModel {
  isPaid: boolean;
  trackingNumber: number;
  amount: number;
  gatewayName: string;
  gatewayAccountName: string;
  transactionCode: string;
  message: string;
}
