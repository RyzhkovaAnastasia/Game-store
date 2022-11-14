import { Role } from "src/app/enums/role";

export class User {
constructor(
    public id: string = "",
    public username: string | null = null,
    public role: string = Role.Guest,
    public email: string = "",
    public publisherId: string | null = null)
    {}
  }