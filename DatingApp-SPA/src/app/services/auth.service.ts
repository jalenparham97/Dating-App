import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { map } from 'rxjs/operators'

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl = 'http://localhost:5000/api/auth'

  constructor(private http: HttpClient) {}

  login(user: any) {
    return this.http.post(`${this.baseUrl}/login`, user).pipe(
      map((res: any) => {
        if (res) {
          localStorage.setItem('token', res.token)
        }
      })
    )
  }

  register(user: any) {
    return this.http.post(`${this.baseUrl}/register`, user)
  }
}
