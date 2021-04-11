import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-payment-result',
  templateUrl: './payment-result.component.html'
})
export class PaymentResultComponent implements OnInit {
  private baseUrl: string;
  private http: HttpClient;

  model: PaymentVerifyResultViewModel;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.http = http;
  }

  ngOnInit(): void {
    this.model = {} as PaymentVerifyResultViewModel;
    this.http.get<PaymentVerifyResultViewModel>(this.baseUrl + 'order').subscribe(result => {
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
