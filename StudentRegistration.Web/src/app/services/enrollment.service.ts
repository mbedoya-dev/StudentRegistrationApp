import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { EnrollStudentPayload, SharedClassStudent, StudentEnrollment } from '../models/enrollment.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EnrollmentService {
  private baseApiUrl = `${environment.apiUrl}/StudentEnrollments`; // Endpoint de StudentEnrollmentsController

  constructor(private http: HttpClient) { }

  enrollStudent(payload: EnrollStudentPayload): Observable<any> {
    return this.http.post(`${this.baseApiUrl}`, payload).pipe(catchError(this.handleError));
  } 

  getStudentEnrollments(studentId: number): Observable<StudentEnrollment[]> {
    return this.http.get<StudentEnrollment[]>(`${this.baseApiUrl}/student/${studentId}`).pipe(catchError(this.handleError));
  }

  getAllStudentEnrollments(): Observable<StudentEnrollment[]> {
    return this.http.get<StudentEnrollment[]>(`${this.baseApiUrl}/all`).pipe(catchError(this.handleError));
  }

  getStudentsInSharedClass(subjectId: number, currentStudentId: number): Observable<SharedClassStudent[]> {
    return this.http.get<SharedClassStudent[]>(`${this.baseApiUrl}/shared-class/${subjectId}/excluding/${currentStudentId}`).pipe(catchError(this.handleError));
  }

  private handleError(error: HttpErrorResponse) {
    console.error('API Error in EnrollmentService:', error);
    let errorMessage = 'An unknown error occurred in Enrollment Service!';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
      if (error.error && typeof error.error === 'string') { // Caso donde el backend devuelve un string simple como error
        errorMessage = error.error;
      } else if (error.error && error.error.title) { // Errores de validación ASP.NET Core
        errorMessage = `${error.error.title}`;
        if (error.error.errors) {
            const validationErrors = Object.values(error.error.errors).flat();
            errorMessage += ` Details: ${validationErrors.join(', ')}`;
        }
      } else if (error.status === 409) { // Conflict
         errorMessage = (typeof error.error === 'string') ? error.error : "Error de conflicto: la operación viola las reglas de negocio.";
      }
    }
    return throwError(() => new Error(errorMessage));
  }  
}