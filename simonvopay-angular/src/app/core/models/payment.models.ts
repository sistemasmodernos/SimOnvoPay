export interface CheckoutInfo {
  token: string;
  amount: number;
  currency: string;
  description: string;
  status: string;
  expiresAt: string;
  metadata?: { [key: string]: string };
}

export interface ProcessPaymentRequest {
  customerName: string;
  customerEmail: string;
  customerPhone: string;
  paymentMethod: 'card' | 'sinpe' | 'sinpe_movil';
  card?: {
    number: string;
    expMonth: number;
    expYear: number;
    cvc: string;
  };
  sinpe?: {
    accountNumber: string;
    accountType: string;
  };
  sinpeMovil?: {
    phone: string;
    identificationType: number;
    identification: string;
  };
}

export interface ProcessPaymentResponse {
  status: 'succeeded' | 'pending' | 'failed' | 'requires_action';
  message: string;
  redirectUrl?: string;
  paymentIntentId?: string;
}

export interface PaymentStatusResponse {
  status: string;
  message?: string;
}
