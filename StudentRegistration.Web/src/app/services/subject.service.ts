import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Subject, SubjectDetail } from '../models/subject.model';
import { Professor } from '../models/professor.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SubjectService {
  private baseApiUrl = `${environment.apiUrl}/Subjects`; // Endpoint de SubjectsController

  constructor(private http: HttpClient) { }

  // Corresponde a GET api/Subjects en SubjectsController.cs
  getAllSubjects(): Observable<Subject[]> {
    return this.http.get<Subject[]>(this.baseApiUrl).pipe(catchError(this.handleError));
  }

  //  Corresponde a GET api/Subjects/with-available-professors en SubjectsController.cs
  getAllSubjectsWithAvailableProfessors(): Observable<SubjectDetail[]> { 
    return this.http.get<SubjectDetail[]>(`${this.baseApiUrl}/with-available-professors`)
      .pipe(catchError(this.handleError));
  }

  // Corresponde a GET api/Subjects/professors en SubjectsController.cs
  getAllProfessors(): Observable<Professor[]> {
    return this.http.get<Professor[]>(`${this.baseApiUrl}/professors`).pipe(catchError(this.handleError));
  }

  private handleError(error: HttpErrorResponse) {
    console.error('API Error in SubjectService:', error);
    let errorMessage = 'An unknown error occurred in Subject Service!';
     if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
      if (error.error && typeof error.error === 'string') {
        errorMessage += `\nDetails: ${error.error}`;
      }
    }
    return throwError(() => new Error(errorMessage));
  }
}