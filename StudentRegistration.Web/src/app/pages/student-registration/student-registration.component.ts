import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

// Importaciones de Angular Material
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar'; 

import { StudentService } from '../../services/student.service'; 
import { CreateStudent, Student } from '../../models/student.model'; 
import { TablerIconsModule } from 'angular-tabler-icons';
import { MatCardModule } from '@angular/material/card';

import { ToastrService, ToastrModule } from 'ngx-toastr';
import { StudentSessionService } from 'src/app/services/student-session.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-student-registration-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule, 
    MatInputModule,
    MatButtonModule,
    MatProgressBarModule, 
    MatCardModule,
    TablerIconsModule,
    ToastrModule,
  ],
  providers: [ToastrService],
  templateUrl: './student-registration.component.html',
})
export class StudentRegistrationComponent implements OnInit {
  studentRegistrationForm!: FormGroup;
  isLoading = false;
  errorMessage: string | null = null;
  successMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private studentService: StudentService,
    private studentSessionService: StudentSessionService, 
    private toastr: ToastrService,
    private router: Router 
  ) {}

  ngOnInit(): void {
    // Al cargar el formulario de cero, borra la sesión del estudiante anterior.
    this.studentSessionService.clearCurrentStudent(); 

    this.studentRegistrationForm = this.fb.group({
      // Basado en CreateStudentDto.cs que espera FirstName, LastName, Email
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  onSubmit(): void {
    if (this.studentRegistrationForm.invalid) {
      this.studentRegistrationForm.markAllAsTouched(); // Muestra errores si el formulario no es válido
      return;
    }

    this.isLoading = true;
    const studentData: CreateStudent = this.studentRegistrationForm.value;

    this.studentService.registerStudent(studentData)
      // El servicio StudentService interactúa con POST /api/Students
      .subscribe({
        next: (newlyRegisteredStudent: Student) => {
          this.isLoading = false;
          this.toastr.success(`¡Estudiante registrado con éxito! ID: ${newlyRegisteredStudent.studentId}`, 'Success!');

          // Guarda el estudiante recién registrado en la sesión
          this.studentSessionService.setCurrentStudent(newlyRegisteredStudent);

          this.studentRegistrationForm.reset(); // Limpia el formulario

          this.router.navigate(['/subject-enrollment']);
        },
        error: (err) => {
          this.isLoading = false;
          if (err.error && typeof err.error === 'string') {
            this.toastr.info(err.error); // Error manejado por el backend
          } else if (err.status === 400) { // Bad Request, podría ser un email duplicado u otro error de validación del backend
            this.toastr.error('Error en los datos enviados. Por favor, verifica la información.', 'Oops!');
          } else {
            this.toastr.warning('Ocurrió un error al registrar el estudiante. Por favor, inténtalo de nuevo más tarde.', 'Alert!');
          }
        }
      });
  }  
}