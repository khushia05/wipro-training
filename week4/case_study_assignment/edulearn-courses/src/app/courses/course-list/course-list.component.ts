import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Course } from '../../models/course';

@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.css'],
})
export class CourseListComponent {
  @Input() courses: Course[] = [];
  @Input() activeCourse?: Course;

  @Output() selectCourse = new EventEmitter<Course>();

  onViewDetails(course: Course) {
    this.selectCourse.emit(course); // EVENT BINDING
  }

  isActive(course: Course) {
    return this.activeCourse && this.activeCourse.id === course.id;
  }

  trackById(index: number, c: Course) {
    return c.id;
  }
}
