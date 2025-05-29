import { Professor } from "./professor.model";

export interface Subject { // Corresponde a SubjectDto.cs
  subjectId: number;
  subjectName: string;
  credits: number;
}

export interface SubjectDetail {
  subjectId: number;
  subjectName: string;
  credits: number;
  availableProfessors: Professor[];
}