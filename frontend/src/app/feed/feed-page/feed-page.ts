import { CommonModule } from '@angular/common';
import { Component, Input, OnInit, inject, signal } from '@angular/core';
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

  readonly post = signal<Post | null>(null);
  readonly loading = signal(true);
  readonly error = signal(false);
  readonly submitting = signal(false);

  readonly currentUserName = CURRENT_USER_NAME;
  readonly currentUserInitial = CURRENT_USER_INITIAL;

  ngOnInit(): void {
    this.loadPost();
  }

  private loadPost(): void {
    this.loading.set(true);
    this.error.set(false);
    this.postService.getPost(this.postId).subscribe({
      next: (post) => {
        this.post.set(post);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
        this.error.set(true);
      },
    });
  }

  onSubmitComment(text: string): void {
    const post = this.post();
    if (!post) {
      return;
    }
    this.submitting.set(true);
    this.commentService.addComment(post.id, text).subscribe({
      next: (comment) => {
        this.post.update((p) => (p ? { ...p, comments: [comment, ...p.comments] } : p));
        this.submitting.set(false);
      },
      error: () => {
        this.submitting.set(false);
      },
    });
  }
}
