export interface Student { // Corresponde a StudentDto.cs
  studentId: number;
  firstName: string;
  lastName: string;
  email: string;
  registrationDate: Date; 
  lastUpdated: Date;    
}

export interface CreateStudent { // Corresponde a CreateStudentDto.cs
  firstName: string;
  lastName: string;
  email: string;
}

export interface UpdateStudent { // Corresponde a UpdateStudentDto.cs
  studentId: number;
  firstName: string;
  lastName: string;
  email: string;
}