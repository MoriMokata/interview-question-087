import { TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { Comment } from '../../core/models/comment.model';
import { CommentItem } from './comment-item';

describe('CommentItem', () => {
  it('renders the author name and comment text', async () => {
    await TestBed.configureTestingModule({
      imports: [CommentItem],
    }).compileComponents();

    const comment: Comment = {
      id: 1,
      postId: 1,
      authorName: 'Blend 285',
      authorInitial: 'B',
      text: 'have a good day',
      createdAt: '2021-10-16T16:05:00Z',
    };

    const fixture = TestBed.createComponent(CommentItem);
    fixture.componentInstance.comment = comment;
    fixture.detectChanges();

    const root = fixture.debugElement.query(By.css('.comment-row')).nativeElement as HTMLElement;
    expect(root.textContent).toContain('Blend 285');
    expect(root.textContent).toContain('have a good day');
    expect(
      fixture.debugElement.query(By.css('.avatar')).nativeElement.textContent.trim(),
    ).toBe('B');
  });
});
