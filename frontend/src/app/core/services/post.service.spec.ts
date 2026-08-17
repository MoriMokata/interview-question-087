import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { API_BASE_URL } from '../config';
import { Post } from '../models/post.model';
import { PostService } from './post.service';

describe('PostService', () => {
  let service: PostService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });
    service = TestBed.inject(PostService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('requests the post by id from the API', () => {
    const mockPost: Post = {
      id: 1,
      authorName: 'Change can',
      authorInitial: 'C',
      content: null,
      imageUrl: 'https://images.example.com/posts/puppy-and-kitten.jpg',
      createdAt: '2021-10-16T16:00:00Z',
      comments: [],
    };

    service.getPost(1).subscribe((post) => {
      expect(post).toEqual(mockPost);
    });

    const req = httpMock.expectOne(`${API_BASE_URL}/posts/1`);
    expect(req.request.method).toBe('GET');
    req.flush(mockPost);
  });
});
