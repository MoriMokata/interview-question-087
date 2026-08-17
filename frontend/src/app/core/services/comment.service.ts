import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config';
import { Comment, CreateCommentRequest } from '../models/comment.model';

@Injectable({ providedIn: 'root' })
export class CommentService {
  private readonly http = inject(HttpClient);

  getComments(postId: number): Observable<Comment[]> {
    return this.http.get<Comment[]>(`${API_BASE_URL}/posts/${postId}/comments`);
  }

  addComment(postId: number, text: string): Observable<Comment> {
    const request: CreateCommentRequest = { text };
    return this.http.post<Comment>(`${API_BASE_URL}/posts/${postId}/comments`, request);
  }
}
