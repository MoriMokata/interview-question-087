import { CommonModule } from '@angular/common';
import { Component, Input, OnInit, inject } from '@angular/core';
import { Post } from '../../core/models/post.model';
import { CommentService } from '../../core/services/comment.service';
import { PostService } from '../../core/services/post.service';
import { CommentInput } from '../comment-input/comment-input';
import { CommentItem } from '../comment-item/comment-item';

/** Current user interacting with the page - there is no login flow in this exercise. */
const CURRENT_USER_NAME = 'Blend 285';
const CURRENT_USER_INITIAL = 'B';

@Component({
  selector: 'app-feed-page',
  standalone: true,
  imports: [CommonModule, CommentItem, CommentInput],
  templateUrl: './feed-page.html',
  styleUrl: './feed-page.scss',
})
export class FeedPage implements OnInit {
  @Input({ required: true }) postId!: number;
  @Input() pageTitle = 'IT 08-1';

  private readonly postService = inject(PostService);
  private readonly commentService = inject(CommentService);

  post: Post | null = null;
  loading = true;
  error = false;
  submitting = false;

  readonly currentUserName = CURRENT_USER_NAME;
  readonly currentUserInitial = CURRENT_USER_INITIAL;

  ngOnInit(): void {
    this.loadPost();
  }

  private loadPost(): void {
    this.loading = true;
    this.error = false;
    this.postService.getPost(this.postId).subscribe({
      next: (post) => {
        this.post = post;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.error = true;
      },
    });
  }

  onSubmitComment(text: string): void {
    if (!this.post) {
      return;
    }
    this.submitting = true;
    this.commentService.addComment(this.post.id, text).subscribe({
      next: (comment) => {
        this.post!.comments = [comment, ...this.post!.comments];
        this.submitting = false;
      },
      error: () => {
        this.submitting = false;
      },
    });
  }
}
