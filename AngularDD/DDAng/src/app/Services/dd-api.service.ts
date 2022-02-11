import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import {Scoreboard} from '../Models/scoreboard';
@Injectable({
  providedIn: 'root'
})
export class DdApiService {

  constructor(private http: HttpClient) { }

  getAllScores() : Promise<Scoreboard[]>
  {
    return firstValueFrom(this.http.get<Scoreboard[]>("http://ddrwebapi-prod.us-west-2.elasticbeanstalk.com/api/scoreboard"))
  }

  getUserById(id : any) :Promise<Scoreboard>
  {
    return firstValueFrom(this.http.get<Scoreboard>(`http://ddrwebapi-prod.us-west-2.elasticbeanstalk.com/api/scoreboard/${id}`))
    
  }

  addNewUser(user : Scoreboard )
  {
    
    return firstValueFrom(this.http.post<any>(`http://ddrwebapi-prod.us-west-2.elasticbeanstalk.com/api/scoreboard`, user))
  }



}
