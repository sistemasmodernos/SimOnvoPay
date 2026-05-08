import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, AbstractControl } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { Subject, interval, takeUntil } from 'rxjs';
import { PaymentService } from '../../core/services/payment.service';
import { CheckoutInfo, ProcessPaymentRequest } from '../../core/models/payment.models';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatTabsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    MatIconModule,
    MatDividerModule
  ],
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit, OnDestroy {
  token = '';
  checkoutInfo: CheckoutInfo | null = null;
  loading = true;
  processing = false;
  error = '';
  paymentState: 'form' | 'processing' | 'pending' | 'error' = 'form';
  pendingMessage = '';
  selectedTab = 0;
  private destroy$ = new Subject<void>();

  cardForm!: FormGroup;
  sinpeForm!: FormGroup;
  sinpeMovilForm!: FormGroup;
  customerForm!: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private paymentService: PaymentService
  ) {}

  ngOnInit(): void {
    this.token = this.route.snapshot.paramMap.get('token') ?? '';
    this.buildForms();
    this.loadCheckoutInfo();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private buildForms(): void {
    this.customerForm = this.fb.group({
      customerName: ['', [Validators.required, Validators.minLength(3)]],
      customerEmail: ['', [Validators.required, Validators.email]],
      customerPhone: ['', [Validators.required, Validators.pattern(/^\+506\d{8}$/)]]
    });

    this.cardForm = this.fb.group({
      number: ['', [Validators.required, Validators.pattern(/^\d{13,19}$/)]],
      expMonth: ['', [Validators.required, Validators.min(1), Validators.max(12)]],
      expYear: ['', [Validators.required, Validators.min(new Date().getFullYear())]],
      cvc: ['', [Validators.required, Validators.pattern(/^\d{3,4}$/)]]
    });

    this.sinpeForm = this.fb.group({
      accountNumber: ['', [Validators.required, Validators.pattern(/^CR\d{20}$/)]],
      accountType: ['checking', Validators.required]
    });

    this.sinpeMovilForm = this.fb.group({
      phone: ['', [Validators.required, Validators.pattern(/^\+506\d{8}$/)]]
    });
  }

  private loadCheckoutInfo(): void {
    this.paymentService.getCheckoutInfo(this.token).subscribe({
      next: (info) => {
        this.checkoutInfo = info;
        this.loading = false;
        if (info.status === 'succeeded') {
          this.router.navigate(['/success'], { queryParams: { token: this.token } });
        } else if (info.status === 'cancelled') {
          this.router.navigate(['/cancelled']);
        }
      },
      error: () => {
        this.loading = false;
        this.error = 'Sesión de pago no válida o expirada.';
      }
    });
  }

  get activePaymentMethod(): 'card' | 'sinpe' | 'sinpe_movil' {
    const methods: ('card' | 'sinpe' | 'sinpe_movil')[] = ['card', 'sinpe', 'sinpe_movil'];
    return methods[this.selectedTab];
  }

  get currentPaymentForm(): FormGroup {
    switch (this.activePaymentMethod) {
      case 'card': return this.cardForm;
      case 'sinpe': return this.sinpeForm;
      case 'sinpe_movil': return this.sinpeMovilForm;
    }
  }

  get isFormValid(): boolean {
    return this.customerForm.valid && this.currentPaymentForm.valid;
  }

  formatAmount(amount: number, currency: string): string {
    return new Intl.NumberFormat('es-CR', { style: 'currency', currency }).format(amount / 100);
  }

  onSubmit(): void {
    if (!this.isFormValid) {
      this.customerForm.markAllAsTouched();
      this.currentPaymentForm.markAllAsTouched();
      return;
    }

    this.processing = true;
    this.paymentState = 'processing';

    const customer = this.customerForm.value;
    const request: ProcessPaymentRequest = {
      customerName: customer.customerName,
      customerEmail: customer.customerEmail,
      customerPhone: customer.customerPhone,
      paymentMethod: this.activePaymentMethod
    };

    if (this.activePaymentMethod === 'card') {
      const c = this.cardForm.value;
      request.card = {
        number: c.number.replace(/\s/g, ''),
        expMonth: Number(c.expMonth),
        expYear: Number(c.expYear),
        cvc: c.cvc
      };
    } else if (this.activePaymentMethod === 'sinpe') {
      request.sinpe = this.sinpeForm.value;
    } else {
      request.sinpeMovil = this.sinpeMovilForm.value;
    }

    this.paymentService.processPayment(this.token, request).subscribe({
      next: (result) => {
        this.processing = false;
        if (result.status === 'succeeded') {
          this.router.navigate(['/success'], { queryParams: { token: this.token } });
        } else if (result.status === 'pending') {
          this.paymentState = 'pending';
          this.pendingMessage = result.message;
          this.startPolling();
        } else if (result.status === 'requires_action' && result.redirectUrl) {
          window.location.href = result.redirectUrl;
        } else {
          this.paymentState = 'error';
          this.error = result.message || 'Error al procesar el pago.';
        }
      },
      error: (err) => {
        this.processing = false;
        this.paymentState = 'error';
        this.error = err?.error?.message || 'Error al procesar el pago. Intente nuevamente.';
      }
    });
  }

  private startPolling(): void {
    interval(4000).pipe(takeUntil(this.destroy$)).subscribe(() => {
      this.paymentService.getStatus(this.token).subscribe(status => {
        if (status.status === 'succeeded') {
          this.destroy$.next();
          this.router.navigate(['/success'], { queryParams: { token: this.token } });
        } else if (status.status === 'failed' || status.status === 'cancelled') {
          this.destroy$.next();
          this.paymentState = 'error';
          this.error = status.message || 'El pago no pudo completarse.';
        }
      });
    });
  }

  onCancel(): void {
    this.paymentService.cancelPayment(this.token).subscribe({
      next: () => this.router.navigate(['/cancelled']),
      error: () => this.router.navigate(['/cancelled'])
    });
  }

  formatCardNumber(event: Event): void {
    const input = event.target as HTMLInputElement;
    const v = input.value.replace(/\s+/g, '').replace(/[^0-9]/gi, '');
    const matches = v.match(/\d{4,16}/g);
    const match = (matches && matches[0]) || '';
    const parts: string[] = [];
    for (let i = 0, len = match.length; i < len; i += 4) {
      parts.push(match.substring(i, i + 4));
    }
    input.value = parts.length ? parts.join(' ') : v;
    this.cardForm.patchValue({ number: v });
  }
}
