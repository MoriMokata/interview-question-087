import { Comment } from './comment.model';

export interface Post {
  id: number;
  authorName: string;
  authorInitial: string;
  content: string | null;
  imageUrl: string | null;
  createdAt: string;
  comments: Comment[];
}
