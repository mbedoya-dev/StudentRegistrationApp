import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { Subscription } from 'rxjs';

// Angular Material
import { MatCheckboxChange, MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

// Modelos
import { Student } from '../../models/student.model';
import { EnrollStudentPayload, SubjectEnrollmentDetail, StudentEnrollment } from '../../models/enrollment.model';

// Servicios
import { SubjectService } from '../../services/subject.service';
import { EnrollmentService } from '../../services/enrollment.service';
import { StudentSessionService } from '../../services/student-session.service';
import { SubjectDetail } from 'src/app/models/subject.model';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { ToastrService } from 'ngx-toastr';

// Modelo local para la UI de selección
export interface EnrollmentSelectionItem {
  subjectDetail: SubjectDetail; // La materia con su lista de availableProfessors
  selectedProfessorId: number | null;
}

@Component({
  selector: 'app-subject-enrollment',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    MatCheckboxModule,
    MatButtonModule,
    MatCardModule,
    MatListModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatSelectModule,
  ],
  providers: [ToastrService],
  templateUrl: './subject-enrollment.component.html' 
})

export class SubjectEnrollmentComponent implements OnInit, OnDestroy {

  loggedInStudent: Student | null = null;
  private studentSubscription!: Subscription;

  allSubjectDetails: SubjectDetail[] = []; // Almacena las materias con sus profesores disponibles

  currentStudentEnrollments: StudentEnrollment[] = [];
  // Maneja las materias que el estudiante ha chequeado y el profesor que ha seleccionado para cada una
  enrollmentSelections: EnrollmentSelectionItem[] = [];

  isLoadingSubjects = false;
  isLoadingEnrollments = false;
  isEnrolling = false;
  initialDataLoaded = false;

  MAX_SUBJECTS_PER_STUDENT = 3;

  constructor(
    private subjectService: SubjectService,
    private enrollmentService: EnrollmentService,
    private studentSessionService: StudentSessionService,
    private cdr: ChangeDetectorRef,
    private router: Router,
    private toastr: ToastrService 
  ) {}

  ngOnInit(): void {
    this.loadSubjectsData();

    this.studentSubscription = this.studentSessionService.currentStudent$.subscribe(student => {
      this.initialDataLoaded = true;
      this.loggedInStudent = student;
      if (this.loggedInStudent) {
        this.loadStudentSpecificData(this.loggedInStudent.studentId);
      } else {
        this.currentStudentEnrollments = [];
        this.enrollmentSelections = [];
        if (this.initialDataLoaded) {
            this.router.navigate(['/']); 
        }
      }
      this.cdr.detectChanges();
    });
  }

  ngOnDestroy(): void {
    if (this.studentSubscription) {
      this.studentSubscription.unsubscribe();
    }
  }

  loadSubjectsData(): void {
    this.isLoadingSubjects = true;
    this.subjectService.getAllSubjectsWithAvailableProfessors().subscribe({ // Llama al método que trae SubjectDetail[]
      next: (subjects) => {
        this.allSubjectDetails = subjects;
        this.isLoadingSubjects = false;
      },
      error: (err) => {
        this.toastr.error('Error al cargar lista de materias: ' + err.message, 'Error');
        this.isLoadingSubjects = false;
      }
    });
  }

  loadStudentSpecificData(studentId: number): void {
    this.enrollmentSelections = []; // Limpiar selecciones al cambiar o cargar estudiante
    this.loadStudentEnrollments(studentId);
  }

  loadStudentEnrollments(studentId: number): void {
    this.isLoadingEnrollments = true;
    this.enrollmentService.getStudentEnrollments(studentId).subscribe({
      next: (enrollments) => {
        this.currentStudentEnrollments = enrollments;
        this.isLoadingEnrollments = false;
      },
      error: (err) => {
        this.toastr.error(`Error cargando inscripciones: ${err.message}`, 'Error');
        this.isLoadingEnrollments = false;
      }
    });
  }

  isSubjectEnrolled(subjectId: number): boolean {
    return this.currentStudentEnrollments.some(e => e.subjectId === subjectId);
  }

  onSubjectCheckboxChange(subjectDetailItem: SubjectDetail, event: MatCheckboxChange): void {
    if (!this.loggedInStudent) return;

    if (event.checked) {
      if (!subjectDetailItem.availableProfessors || subjectDetailItem.availableProfessors.length === 0) {
        this.toastr.warning(`La materia '${subjectDetailItem.subjectName}' no tiene profesores disponibles y no puede ser seleccionada.`, 'Advertencia');
        event.source.checked = false; // Desmarcar visualmente el checkbox
        this.cdr.detectChanges();
        return;
      }

      const canSelectMore = (this.currentStudentEnrollments.length + this.enrollmentSelections.length) < this.MAX_SUBJECTS_PER_STUDENT;
      if (canSelectMore) {
        if (!this.enrollmentSelections.find(es => es.subjectDetail.subjectId === subjectDetailItem.subjectId)) {
            this.enrollmentSelections.push({
              subjectDetail: subjectDetailItem,
              selectedProfessorId: null // Se seleccionará después
            });
        }
      } else {
        this.toastr.info(`Solo puedes seleccionar hasta ${this.MAX_SUBJECTS_PER_STUDENT - this.currentStudentEnrollments.length} materias nuevas.`, 'Límite Alcanzado');
        event.source.checked = false;
        this.cdr.detectChanges();
      }
    } else {
      this.enrollmentSelections = this.enrollmentSelections.filter(es => es.subjectDetail.subjectId !== subjectDetailItem.subjectId);
    }
  }
  
  isSubjectSelectedForEnrollment(subjectId: number): boolean {
    return this.enrollmentSelections.some(es => es.subjectDetail.subjectId === subjectId);
  }

  // Verifica si hay materias seleccionadas que aún no tienen un profesor elegido
  get hasUnselectedProfessors(): boolean {
    if (this.enrollmentSelections.length === 0) return false;
    return this.enrollmentSelections.some(selItem => selItem.selectedProfessorId === null);
  }

  isEnrollButtonDisabled(): boolean {
    return this.isEnrolling || this.enrollmentSelections.length === 0 || this.hasUnselectedProfessors;
  }

  prepareAndEnrollStudent(): void {
    if (!this.loggedInStudent) {
      return;
    }

    if (this.hasUnselectedProfessors) {
      this.toastr.warning('Por favor, seleccione un profesor para cada materia marcada.', 'Acción Requerida');
      return;
    }
    
    const payloadEnrollments: SubjectEnrollmentDetail[] = this.enrollmentSelections.map(selItem => ({
      subjectId: selItem.subjectDetail.subjectId,
      professorId: selItem.selectedProfessorId! // El ! es seguro por la validación de hasUnselectedProfessors
    }));

    if (payloadEnrollments.length === 0) {
      this.toastr.info('No hay materias seleccionadas para inscribir.', 'Información');
      return;
    }
        
    if (this.currentStudentEnrollments.length + payloadEnrollments.length > this.MAX_SUBJECTS_PER_STUDENT) {
        this.toastr.error(`Error: El estudiante no puede estar inscrito en más de ${this.MAX_SUBJECTS_PER_STUDENT} materias en total.`, 'Límite Excedido');
        return;
    }

    // REGLA: El estudiante no podrá tener clases con el mismo profesor.
    // 1. Verificar dentro de la selección actual
    const selectedProfessorIds = payloadEnrollments.map(e => e.professorId);
    const uniqueSelectedProfessorIds = new Set(selectedProfessorIds);
    if (selectedProfessorIds.length !== uniqueSelectedProfessorIds.size) {
        this.toastr.error('Error: Ha seleccionado el mismo profesor para múltiples materias. Por favor, elija profesores diferentes.', 'Conflicto de Profesor');
        return;
    }

    // 2. Verificar contra las materias ya inscritas
    const enrolledProfessorIds = new Set(this.currentStudentEnrollments.map(e => e.professorId));
    for (const enrollmentDetail of payloadEnrollments) {
        if (enrolledProfessorIds.has(enrollmentDetail.professorId)) {
            const selectionItem = this.enrollmentSelections.find(s => s.subjectDetail.subjectId === enrollmentDetail.subjectId);
            const professor = selectionItem?.subjectDetail.availableProfessors.find(p => p.professorId === enrollmentDetail.professorId);
            this.toastr.error(`Error: El estudiante ya tiene una clase con el profesor ${professor?.firstName} ${professor?.lastName} (para la materia ${selectionItem?.subjectDetail.subjectName}). No puede repetir profesor.`, 'Conflicto de Profesor');
            return;
        }
    }

    const payload: EnrollStudentPayload = {
      studentId: this.loggedInStudent.studentId,
      enrollments: payloadEnrollments
    };

    this.isEnrolling = true;
    this.enrollmentService.enrollStudent(payload).subscribe({
      next: (response: any) => { // Especificar 'any' o el tipo de respuesta esperado
        this.toastr.success('Inscripción realizada exitosamente!', 'Éxito');        
        this.isEnrolling = false;
        if(this.loggedInStudent) {
            this.loadStudentEnrollments(this.loggedInStudent.studentId);
        }
        this.enrollmentSelections = []; // Limpiar las selecciones para una nueva operación
        this.cdr.detectChanges(); // Para refrescar el estado de los checkboxes y selects
      },
      error: (err) => {
        this.toastr.error('Error en la inscripción: ' + err.message, 'Error');
        this.isEnrolling = false;
      }
    });
  }

  showShareClass(): void {
    this.router.navigate(['/shared-class']);
  }
}
