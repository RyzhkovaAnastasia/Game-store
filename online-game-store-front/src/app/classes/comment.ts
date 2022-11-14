export class Comment {
  [prop: string]: any;
  constructor(
  public id: string,
  public name: string,
  public body: string | null,
  public isQuoted: boolean,
  public gameId: string | null,
  public parentCommentId: string | null,
  public parentCommentName: string | null,
  public parentCommentBody: string | null,
  public childComments: Comment[] | null)
  {

  }
}