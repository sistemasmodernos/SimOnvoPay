import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-success',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule],
  template: `
    <div class="page-wrapper success">
      <div class="state-card">
        <div class="icon-circle success-circle">
          <mat-icon>check_circle</mat-icon>
        </div>
        <h1>¡Pago Exitoso!</h1>
        <p>Su pago fue procesado correctamente.</p>
        <p *ngIf="token" class="ref">Referencia: {{ token }}</p>
        <p class="note">Puede cerrar esta ventana.</p>
      </div>
    </div>
  `,
  styles: [`
    .page-wrapper {
      min-height: 100vh;
      display: flex;
      align-items: center;
      justify-content: center;
      padding: 24px;
    }
    .success { background: linear-gradient(135deg, #56ab2f 0%, #a8e063 100%); }
    .state-card {
      background: white;
      border-radius: 16px;
      padding: 56px 48px;
      text-align: center;
      max-width: 420px;
      box-shadow: 0 25px 50px rgba(0,0,0,0.2);
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 12px;
    }
    .icon-circle {
      width: 96px; height: 96px;
      border-radius: 50%;
      display: flex; align-items: center; justify-content: center;
      margin-bottom: 8px;
      mat-icon { font-size: 56px; height: 56px; width: 56px; }
    }
    .success-circle { background: #e8f5e9; mat-icon { color: #2e7d32; } }
    h1 { margin: 0; color: #212529; }
    p { margin: 0; color: #6c757d; }
    .ref { font-family: monospace; font-size: 13px; background: #f8f9fa; padding: 6px 12px; border-radius: 6px; }
    .note { font-size: 13px; }
  `]
})
export class SuccessComponent implements OnInit {
  token = '';
  constructor(private route: ActivatedRoute) {}
  ngOnInit(): void {
    this.token = this.route.snapshot.queryParams['token'] ?? '';
  }
}
