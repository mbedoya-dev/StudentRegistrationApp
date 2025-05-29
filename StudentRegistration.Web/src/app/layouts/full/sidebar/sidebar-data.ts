import { NavItem } from './nav-item/nav-item';

export const navItems: NavItem[] = [
  {
    navCap: 'Estuiantes',
  },
  {
    displayName: 'Inicio',
    iconName: 'home',
    route: '/starter',
  },
  {
    displayName: 'Registrar Estudiante',
    iconName: 'user-edit', 
    route: '/student/student-registration',
  },
  {
    navCap: 'CRUD',
  },
];
