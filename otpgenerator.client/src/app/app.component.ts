import { Component } from '@angular/core';
import { AppService, OtpResponse } from './app.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  otpResponse: OtpResponse | null = null;
  remainingTime: number | null = null;
  intervalId: any;
  showDateTimeInput: boolean = true;
  selectedDateTime: string = '';
  constructor(private appService: AppService) {}

  requestOtp(userId: string): void {
    this.appService.generateOtp({ userId }).subscribe(response => {
      this.otpResponse = response;
      this.startCountdown();
    });
  }

  onDateTimeChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.selectedDateTime = input.value;
    this.showDateTimeInput = false;
  }

  private startCountdown(): void {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }

    this.intervalId = setInterval(() => {
      if (this.otpResponse) {
        const now = new Date();
        const validUntil = new Date(this.otpResponse.validUntil);
        this.remainingTime = Math.floor((validUntil.getTime() - now.getTime()) / 1000);

        if (this.remainingTime <= 0) {
          clearInterval(this.intervalId);
          this.remainingTime = null;
        }
      }
    }, 1000);
  }
}
