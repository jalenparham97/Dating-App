import { Component, OnInit } from '@angular/core'
import { AuthService } from 'src/app/services/auth.service'

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
  user: any = {}

  constructor(private authService: AuthService) {}

  ngOnInit() {}

  login() {
    this.authService.login(this.user).subscribe(
      (next) => {
        console.log('User Logged in')
      },
      (error) => {
        console.log('Failed to login')
      }
    )
  }

  loggedIn() {
    const token: string = localStorage.getItem('token')
    return !!token
  }

  logoutOut() {
    localStorage.removeItem('token')
    console.log('Logged Out')
  }
}
