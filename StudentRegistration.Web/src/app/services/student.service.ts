import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Student, CreateStudent } from '../models/student.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  private baseApiUrl = `${environment.apiUrl}/Students`; // Endpoint de StudentsController

  constructor(private http: HttpClient) { }

  registerStudent(studentData: CreateStudent): Observable<Student> {
    return this.http.post<Student>(this.baseApiUrl, studentData).pipe(catchError(this.handleError));
  }

  getStudents(): Observable<Student[]> {
    return this.http.get<Student[]>(this.baseApiUrl).pipe(catchError(this.handleError));
  }

  getStudentById(id: number): Observable<Student> {
    return this.http.get<Student>(`${this.baseApiUrl}/${id}`).pipe(catchError(this.handleError));
  }

  // Añade aquí updateStudent y deleteStudent si los necesitas en este componente o en otros.

  private handleError(error: HttpErrorResponse) {
    // Implementa un manejo de errores robusto y consistente
    console.error('API Error in StudentService:', error);
    let errorMessage = 'An unknown error occurred in Student Service!';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
      if (error.error && error.error.title) { // ASP.NET Core validation errors
        errorMessage += `\n${error.error.title}`;
        if (error.error.errors) {
            const validationErrors = Object.values(error.error.errors).flat();
            errorMessage += `\nDetails: ${validationErrors.join(', ')}`;
        }
      } else if (typeof error.error === 'string') {
        errorMessage += `\nDetails: ${error.error}`;
      }
    }
    return throwError(() => new Error(errorMessage));
  }
}