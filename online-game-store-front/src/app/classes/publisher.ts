export class Publisher {
  [prop: string]: any;
  constructor(
  public id: string = '',
  public companyName: string = '',
  public description: string = '',
  public homePage: string =''
  ) { }
}