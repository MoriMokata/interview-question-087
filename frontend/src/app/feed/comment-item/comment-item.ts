import { Component, Input } from '@angular/core';
import { Comment } from '../../core/models/comment.model';

@Component({
  selector: 'app-comment-item',
  standalone: true,
  templateUrl: './comment-item.html',
  styleUrl: './comment-item.scss',
})
export class CommentItem {
  @Input({ required: true }) comment!: Comment;
}
