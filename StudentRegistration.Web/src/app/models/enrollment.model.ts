import { Professor } from "./professor.model";
import { Subject } from "./subject.model";

export interface SubjectEnrollmentDetail { // Para el payload de inscripción
  subjectId: number;
  professorId: number;
}

export interface EnrollStudentPayload { // Payload para POST /api/StudentEnrollments/enroll
  studentId: number;
  enrollments: SubjectEnrollmentDetail[];
}

export interface StudentEnrollment { // Para GET /api/StudentEnrollments/student/{studentId} y GET /api/StudentEnrollments/all
  studentSubjectId: number;
  studentId: number;
  studentFirstName: string;
  studentLastName: string;
  subjectId: number;
  subjectName: string;
  professorId: number;
  professorFirstName: string;
  professorLastName: string;
  enrollmentDate: string;
}

export interface SharedClassStudent { // Para GET /api/StudentEnrollments/shared-class/{subjectId}/excluding/{currentStudentId}
  studentId: number;
  firstName: string;
  lastName: string;
}

// Modelo local para la UI, para manejar la selección de materias y profesores
export interface EnrollmentSelectionItem {
  subject: Subject;
  availableProfessors: Professor[]; // Profesores que pueden dictar esta materia 
  selectedProfessorId: number | null;
}