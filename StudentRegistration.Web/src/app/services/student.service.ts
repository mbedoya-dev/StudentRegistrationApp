import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Student, CreateStudent } from '../models/student.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  private baseApiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // Corresponde a POST api/Students en StudentsController.cs
  registerStudent(studentData: CreateStudent): Observable<Student> {
    return this.http.post<Student>(`${this.baseApiUrl}/Students`, studentData);
  }

  getStudents(): Observable<Student[]> { // Corresponde a GET api/Students
    return this.http.get<Student[]>(`${this.baseApiUrl}/Students`);
  }

  getStudentById(id: number): Observable<Student> {
    return this.http.get<Student>(`${this.baseApiUrl}/Students/${id}`);
  }
}