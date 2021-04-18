import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-payment-request',
  templateUrl: './payment-request.component.html'
})
export class PaymentRequestComponent implements OnInit {
  private baseUrl: string;
  private http: HttpClient;

  model: PayViewModel;
  gateways: Gateway[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.http = http;
    this.model = new PayViewModel();
    this.model.generateTrackingNumberAutomatically = true;
  }

  ngOnInit(): void {
    this.http.get<Gateway[]>(this.baseUrl + 'payment/gateways').subscribe(result => {
      this.gateways = result;
    }, error => console.error(error));
  }

  pay() {
    this.http.post<PaymentRequestResultViewModel>(this.baseUrl + 'payment/pay', this.model).subscribe(result => {
      if (!result.isSucceed) {
        alert(result.message);
        return;
      }

      this.transportToGateway(result.transporter);

    }, error => console.error(error));
  }

  transportToGateway(transporter: GatewayTransporter) {
    if (transporter.type === TransportType.Redirect) {
      // Transporting with Redirect
      window.location.href = transporter.url;
    } else {
      // Transporting with Form
      const form = document.createElement('form');
      form.setAttribute('id', 'myForm');
      form.setAttribute('method', 'post');
      form.setAttribute('action', transporter.url);

      transporter.form.forEach(item => {
        const input = document.createElement('input');
        input.setAttribute('type', 'hidden');
        input.setAttribute('name', item.key);
        input.setAttribute('value', item.value);
        form.appendChild(input);
      });

      document.getElementsByTagName('body')[0].appendChild(form);
      (document.getElementById('myForm')as HTMLFormElement).submit();
    }
  }
}

class PayViewModel {
  trackingNumber: number;
  generateTrackingNumberAutomatically: boolean;
  amount: number;
  selectedGateway: number;
}

interface Gateway {
  name: string;
  value: number;
}

interface PaymentRequestResultViewModel {
  isSucceed: boolean;
  message: string;
  transporter: GatewayTransporter;
}

interface GatewayTransporter {
  type: TransportType;
  url: string;
  form: KeyValuePair[];
}

interface KeyValuePair {
  key: any;
  value: any;
}

enum TransportType {
  Post,
  Redirect
}
