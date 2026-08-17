export interface Comment {
  id: number;
  postId: number;
  authorName: string;
  authorInitial: string;
  text: string;
  createdAt: string;
}

/** Payload sent when submitting a new comment (Enter key). */
export interface CreateCommentRequest {
  text: string;
}
