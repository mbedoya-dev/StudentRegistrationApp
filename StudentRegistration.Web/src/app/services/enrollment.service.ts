import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EnrollStudentPayload } from '../models/enrollment.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EnrollmentService {
  private baseApiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // Corresponde a POST api/StudentEnrollments/enroll en StudentEnrollmentsController.cs
  enrollStudent(payload: EnrollStudentPayload): Observable<any> { // El backend devuelve Ok() o un error.
    return this.http.post(`${this.baseApiUrl}/StudentEnrollments/enroll`, payload);
  }
}