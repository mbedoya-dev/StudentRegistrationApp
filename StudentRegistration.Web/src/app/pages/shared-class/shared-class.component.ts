import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTableDataSource, MatTableModule } from '@angular/material/table'; 
import { Router } from '@angular/router';
import { TablerIconsModule } from 'angular-tabler-icons';
import { ToastrModule } from 'ngx-toastr'; 
import { Subscription } from 'rxjs';
import { SharedClassStudent, StudentEnrollment } from 'src/app/models/enrollment.model';
import { Student } from 'src/app/models/student.model';
import { EnrollmentService } from 'src/app/services/enrollment.service';
import { StudentSessionService } from 'src/app/services/student-session.service';

// Interfaz para la estructura de datos de la tabla
export interface SharedClassRow {
  firstName: string;
  lastName: string;
  sharedClassName: string;
}

@Component({
  selector: 'app-shared-class',
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressBarModule,
    MatCardModule,
    MatTableModule, 
    TablerIconsModule,
    ToastrModule,
  ],
  templateUrl: './shared-class.component.html',
})
export class SharedClassComponent implements OnInit, OnDestroy {

  currentStudentName: string = 'Cargando...';

  displayedColumns: string[] = ['alumno', 'claseComun'];
  dataSource = new MatTableDataSource<SharedClassRow>([]);

  private studentSubscription: Subscription | undefined;
  private initialDataLoaded: boolean = false;
  loggedInStudent: Student | null = null;

  constructor(
    private enrollmentService: EnrollmentService,
    private studentSessionService: StudentSessionService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.studentSubscription = this.studentSessionService.currentStudent$.subscribe(student => {
      if (!this.initialDataLoaded) {
        this.initialDataLoaded = true;
      }
      this.loggedInStudent = student;
      if (this.loggedInStudent && this.loggedInStudent.studentId) {
        this.currentStudentName = `${this.loggedInStudent.firstName} ${this.loggedInStudent.lastName}`;
        this.loadSharedClassmates(this.loggedInStudent.studentId);
      } else {
        this.currentStudentName = 'No hay estudiante en sesión';
        this.dataSource.data = [];
        if (this.initialDataLoaded) {
          this.router.navigate(['/']);
        }
      }
      this.cdr.detectChanges();
    });
  }

  async loadSharedClassmates(currentStudentId: number): Promise<void> {
    if (!currentStudentId) {
      this.dataSource.data = [];
      return;
    }
    const combinedData: SharedClassRow[] = [];
    try {
      const enrollments: StudentEnrollment[] = await this.enrollmentService.getStudentEnrollments(currentStudentId).toPromise() || [];
      for (const enrollment of enrollments) {
        if (enrollment.subjectId) {
          const sharedStudents: SharedClassStudent[] = await this.enrollmentService.getStudentsInSharedClass(enrollment.subjectId, currentStudentId).toPromise() || [];
          for (const student of sharedStudents) {
            combinedData.push({
              firstName: student.firstName,
              lastName: student.lastName,
              sharedClassName: enrollment.subjectName || 'Clase Desconocida'
            });
          }
        }
      }
      this.dataSource.data = combinedData;
    } catch (error) {
      console.error("Error cargando compañeros de clase:", error);
      this.dataSource.data = [];
    }
    this.cdr.detectChanges();
  }

  ngOnDestroy(): void {
    if (this.studentSubscription) {
      this.studentSubscription.unsubscribe();
    }
  }
}