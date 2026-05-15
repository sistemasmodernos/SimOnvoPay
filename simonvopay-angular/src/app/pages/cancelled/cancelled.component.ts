import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-cancelled',
  standalone: true,
  imports: [MatCardModule, MatButtonModule, MatIconModule],
  template: `
    <div class="page-wrapper cancelled">
      <div class="state-card">
        <div class="icon-circle cancel-circle">
          <mat-icon>cancel</mat-icon>
        </div>
        <h1>Pago Cancelado</h1>
        <p>Ha cancelado el proceso de pago.</p>
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
    .cancelled { background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%); }
    .state-card {
      background: white;
      border-radius: 16px;
      padding: 56px 48px;
      text-align: center;
      max-width: 420px;
      box-shadow: 0 25px 50px rgba(0,0,0,0.15);
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
    .cancel-circle { background: #fff3e0; mat-icon { color: #e65100; } }
    h1 { margin: 0; color: #212529; }
    p { margin: 0; color: #6c757d; }
    .note { font-size: 13px; }
  `]
})
export class CancelledComponent {}
