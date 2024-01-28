import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface OtpRequest {
  userId: string;
}

export interface OtpResponse {
  otp: string;
  validUntil: Date;
}

@Injectable({
  providedIn: 'root'
})
export class AppService {
  private apiUrl = 'https://localhost:7285/api/Otp';

  constructor(private http: HttpClient) {}

  generateOtp(request: OtpRequest): Observable<OtpResponse> {
    return this.http.post<OtpResponse>(`${this.apiUrl}/generate`, request);
  }
}
