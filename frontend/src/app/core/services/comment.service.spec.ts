import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { API_BASE_URL } from '../config';
import { Comment } from '../models/comment.model';
import { CommentService } from './comment.service';

describe('CommentService', () => {
  let service: CommentService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });
    service = TestBed.inject(CommentService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('requests comments for a post', () => {
    const mockComments: Comment[] = [
      {
        id: 1,
        postId: 1,
        authorName: 'Blend 285',
        authorInitial: 'B',
        text: 'have a good day',
        createdAt: '2021-10-16T16:05:00Z',
      },
    ];

    service.getComments(1).subscribe((comments) => {
      expect(comments).toEqual(mockComments);
    });

    const req = httpMock.expectOne(`${API_BASE_URL}/posts/1/comments`);
    expect(req.request.method).toBe('GET');
    req.flush(mockComments);
  });

  it('posts a new comment with the given text', () => {
    const created: Comment = {
      id: 2,
      postId: 1,
      authorName: 'Blend 285',
      authorInitial: 'B',
      text: 'nice photo',
      createdAt: '2021-10-16T16:10:00Z',
    };

    service.addComment(1, 'nice photo').subscribe((comment) => {
      expect(comment).toEqual(created);
    });

    const req = httpMock.expectOne(`${API_BASE_URL}/posts/1/comments`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ text: 'nice photo' });
    req.flush(created);
  });
});
