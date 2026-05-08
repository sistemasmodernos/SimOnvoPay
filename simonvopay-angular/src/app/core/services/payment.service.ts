import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CheckoutInfo, ProcessPaymentRequest, ProcessPaymentResponse, PaymentStatusResponse } from '../models/payment.models';

@Injectable({ providedIn: 'root' })
export class PaymentService {
  private readonly base = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getCheckoutInfo(token: string): Observable<CheckoutInfo> {
    return this.http.get<CheckoutInfo>(`${this.base}/api/v1/checkout/${token}`);
  }

  processPayment(token: string, request: ProcessPaymentRequest): Observable<ProcessPaymentResponse> {
    return this.http.post<ProcessPaymentResponse>(`${this.base}/api/v1/checkout/${token}/pay`, request);
  }

  cancelPayment(token: string): Observable<void> {
    return this.http.post<void>(`${this.base}/api/v1/checkout/${token}/cancel`, {});
  }

  getStatus(token: string): Observable<PaymentStatusResponse> {
    return this.http.get<PaymentStatusResponse>(`${this.base}/api/v1/checkout/${token}/status`);
  }
}
