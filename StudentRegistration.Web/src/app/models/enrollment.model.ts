import { Professor } from "./professor.model";
import { Subject } from "./subject.model";

// Basado en EnrollStudentDto.cs
export interface EnrollStudentPayload {
  studentId: number;
  subjectIds: number[];
  professorIds: number[]; // Asegurar que el orden coincida con subjectIds
}

// Modelo local para manejar la selección en el frontend antes de enviar
export interface SelectedEnrollmentItem {
  subject: Subject;
  selectedProfessor: Professor | null;
  availableProfessors: Professor[];
}