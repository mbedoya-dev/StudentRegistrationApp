import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Subject } from '../models/subject.model';
import { Professor } from '../models/professor.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SubjectService {
  private baseApiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // Corresponde a GET api/Subjects en SubjectsController.cs
  getSubjects(): Observable<Subject[]> {
    return this.http.get<Subject[]>(this.baseApiUrl);
  }

  // Corresponde a GET api/Subjects/professors en SubjectsController.cs
  getProfessors(subjectId: number): Observable<Professor[]> {
    return this.http.get<Professor[]>(`${this.baseApiUrl}/Subjects/professors`);
  }
}