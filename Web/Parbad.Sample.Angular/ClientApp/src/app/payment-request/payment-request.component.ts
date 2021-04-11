import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-payment-request',
  templateUrl: './payment-request.component.html'
})
export class PaymentRequestComponent {
  private baseUrl: string;
  private http: HttpClient;

  model: PayViewModel;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.http = http;
    this.model = new PayViewModel();
    this.model.generateTrackingNumberAutomatically = true;
    this.model.selectedGateway = 'ParbadVirtual';
  }

  pay() {
    this.http.post<PaymentRequestResultViewModel>(this.baseUrl + 'payment/pay', this.model).subscribe(result => {
      if (!result.isSucceed) {
        alert(result.message);
        return;
      }

      this.transportToGateway(result.gatewayTransporter.descriptor);

    }, error => console.error(error));
  }

  transportToGateway(descriptor: GatewayTransporterDescriptor) {
    if (descriptor.type === TransportType.Redirect) {
      // Transporting with Redirect
      window.location.href = descriptor.url;
    } else {
      // Transporting with Form
      const form = document.createElement('form');
      form.setAttribute('id', 'myForm');
      form.setAttribute('method', 'post');
      form.setAttribute('action', descriptor.url);

      descriptor.form.forEach(item => {
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
  selectedGateway: string;
}

interface PaymentRequestResultViewModel {
  isSucceed: boolean;
  trackingNumber: number;
  amount: number;
  message: string;
  gatewayTransporter: GatewayTransporter;
}

interface GatewayTransporter {
  descriptor: GatewayTransporterDescriptor;
}

interface GatewayTransporterDescriptor {
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
