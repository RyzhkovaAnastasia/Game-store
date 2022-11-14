import * as moment from "moment";
import { Observable } from "rxjs";

class Cache {
  constructor(
  public expires: Date,
  public value: Observable<number>
  ){} 
}

export class GameCacheService {

  readonly CACHE_DURATION_IN_MINUTES = 1;

  cache: Cache | null = null;
  constructor() {
  }

  getValue(): Observable<number> | null{
    if (this.cache == null) {
      return null;
    }

    if (moment(new Date()).isAfter(this.cache.expires)) {
      return null;
    }

    return this.cache.value;
  }

  setValue(value: Observable<number>) {
    this.cache = {
      value,
      expires: moment(new Date()).add(this.CACHE_DURATION_IN_MINUTES, 'minutes').toDate()
    };

  }

  clearCache() {
    this.cache = null;
  }
}