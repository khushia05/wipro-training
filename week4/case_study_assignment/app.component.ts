import { Component } from '@angular/core';
import { Course } from './models/course';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'EduLearn Course Manager';

  courses: Course[] = [
    {
      id: 1,
      title: 'Angular Basics',
      description: 'Learn components, templates, and data binding.',
      category: 'Web Development',
      level: 'Beginner'
    },
    {
      id: 2,
      title: 'Node.js Fundamentals',
      description: 'Build server-side apps with Express and APIs.',
      category: 'Backend',
      level: 'Intermediate'
    },
    {
      id: 3,
      title: 'TypeScript Mastery',
      description: 'Generics, advanced types, and best practices.',
      category: 'Programming',
      level: 'Advanced'
    }
  ];

  selectedCourse?: Course;

  onSelectCourse(course: Course) {
    // keep the same object reference so edits reflect in list automatically
    this.selectedCourse = course;
  }

  // optional: clear selection
  clearSelection() {
    this.selectedCourse = undefined;
  }
}
