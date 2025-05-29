import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Student } from '../models/student.model'; 

@Injectable({
  providedIn: 'root'
})
export class StudentSessionService {
  // BehaviorSubject para mantener el estado actual del estudiante y emitir a los suscriptores.
  private currentStudentSubject = new BehaviorSubject<Student | null>(null);
  
  // Observable público para que los componentes se suscriban.
  public currentStudent$: Observable<Student | null> = this.currentStudentSubject.asObservable();

  constructor() { }

  setCurrentStudent(student: Student): void {
    this.currentStudentSubject.next(student);
  }

  getCurrentStudentValue(): Student | null {
    return this.currentStudentSubject.getValue();
  }

  clearCurrentStudent(): void {
    this.currentStudentSubject.next(null);
  }

  isStudentLoggedIn(): boolean {
    return !!this.currentStudentSubject.getValue();
  }
}