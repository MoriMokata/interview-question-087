import { TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { of, throwError } from 'rxjs';
import { Post } from '../../core/models/post.model';
import { CommentService } from '../../core/services/comment.service';
import { PostService } from '../../core/services/post.service';
import { FeedPage } from './feed-page';

describe('FeedPage', () => {
  const samplePost: Post = {
    id: 1,
    authorName: 'Change can',
    authorInitial: 'C',
    content: null,
    imageUrl: 'https://i.pinimg.com/1200x/58/b8/94/58b894d8c2f1bfd5056362933f9bb056.jpg',
    createdAt: '2021-10-16T16:00:00Z',
    comments: [
      {
        id: 1,
        postId: 1,
        authorName: 'Blend 285',
        authorInitial: 'B',
        text: 'have a good day',
        createdAt: '2021-10-16T16:05:00Z',
      },
    ],
  };

  async function setup(postServiceStub: Partial<PostService>, commentServiceStub: Partial<CommentService> = {}) {
    await TestBed.configureTestingModule({
      imports: [FeedPage],
      providers: [
        { provide: PostService, useValue: postServiceStub },
        { provide: CommentService, useValue: commentServiceStub },
      ],
    }).compileComponents();

    const fixture = TestBed.createComponent(FeedPage);
    fixture.componentInstance.postId = 1;
    fixture.detectChanges();
    return fixture;
  }

  it('shows a loading state, then renders the post and its comments', async () => {
    const fixture = await setup({ getPost: () => of(samplePost) });

    expect(fixture.debugElement.query(By.css('.page-header')).nativeElement.textContent).toContain('IT 08-1');
    expect(fixture.debugElement.query(By.css('.post-block'))).toBeTruthy();
    expect(fixture.debugElement.queryAll(By.css('app-comment-item')).length).toBe(1);
  });

  it('shows an error message when the post fails to load', async () => {
    const fixture = await setup({ getPost: () => throwError(() => new Error('network error')) });

    expect(fixture.debugElement.query(By.css('.state-error'))).toBeTruthy();
  });

  it('prepends a newly submitted comment above the existing ones', async () => {
    const newComment = {
      id: 2,
      postId: 1,
      authorName: 'Blend 285',
      authorInitial: 'B',
      text: 'nice photo',
      createdAt: '2021-10-16T16:10:00Z',
    };

    const fixture = await setup(
      { getPost: () => of({ ...samplePost, comments: [...samplePost.comments] }) },
      { addComment: () => of(newComment) },
    );

    fixture.componentInstance.onSubmitComment('nice photo');
    fixture.detectChanges();

    expect(fixture.componentInstance.post()?.comments[0].text).toBe('nice photo');
    expect(fixture.componentInstance.post()?.comments.length).toBe(2);
  });
});
