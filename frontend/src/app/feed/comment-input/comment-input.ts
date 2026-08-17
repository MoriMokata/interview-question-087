import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-comment-input',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './comment-input.html',
  styleUrl: './comment-input.scss',
})
export class CommentInput {
  @Input() authorName = '';
  @Input() authorInitial = '';
  @Input() submitting = false;
  @Output() submitted = new EventEmitter<string>();

  text = '';

  onEnter(): void {
    const trimmed = this.text.trim();
    if (!trimmed || this.submitting) {
      return;
    }
    this.submitted.emit(trimmed);
    this.text = '';
  }
}
