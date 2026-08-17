import { TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { CommentInput } from './comment-input';

describe('CommentInput', () => {
  const setup = async () => {
    await TestBed.configureTestingModule({
      imports: [CommentInput],
    }).compileComponents();

    const fixture = TestBed.createComponent(CommentInput);
    fixture.componentInstance.authorName = 'Blend 285';
    fixture.componentInstance.authorInitial = 'B';
    fixture.detectChanges();
    return fixture;
  };

  it('emits the trimmed text and clears the box on Enter', async () => {
    const fixture = await setup();
    const component = fixture.componentInstance;
    const emitted: string[] = [];
    component.submitted.subscribe((text) => emitted.push(text));

    component.text = '  have a good day  ';
    const input = fixture.debugElement.query(By.css('input')).nativeElement as HTMLInputElement;
    input.dispatchEvent(new KeyboardEvent('keyup', { key: 'Enter' }));
    fixture.detectChanges();

    expect(emitted).toEqual(['have a good day']);
    expect(component.text).toBe('');
  });

  it('does not emit for empty or whitespace-only text', async () => {
    const fixture = await setup();
    const component = fixture.componentInstance;
    const emitted: string[] = [];
    component.submitted.subscribe((text) => emitted.push(text));

    component.text = '   ';
    component.onEnter();

    expect(emitted).toEqual([]);
  });

  it('does not emit while a submission is already in progress', async () => {
    const fixture = await setup();
    const component = fixture.componentInstance;
    component.submitting = true;
    const emitted: string[] = [];
    component.submitted.subscribe((text) => emitted.push(text));

    component.text = 'hello';
    component.onEnter();

    expect(emitted).toEqual([]);
  });

  it('renders the current user name', async () => {
    const fixture = await setup();
    const nameEl = fixture.debugElement.query(By.css('.author-name')).nativeElement as HTMLElement;
    expect(nameEl.textContent).toContain('Blend 285');
  });
});
