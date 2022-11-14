export class GamePagination {
  [prop: string]: any;
  constructor(
    public currentPage: number = 1,
    public itemsPerPage: number = 10
  ){}
}