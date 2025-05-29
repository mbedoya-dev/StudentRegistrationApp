// Basado en CreateStudentDto.cs y StudentDto.cs
export interface CreateStudent {
  firstName: string;
  lastName: string;
  email: string;
}

export interface Student extends CreateStudent {
  studentId: number;
}