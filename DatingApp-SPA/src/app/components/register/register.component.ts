import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core'
import { AuthService } from 'src/app/services/auth.service'

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter()
  user: any = {}

  constructor(private authService: AuthService) {}

  ngOnInit() {}

  register() {
    this.authService.register(this.user).subscribe(
      () => {
        console.log('Registration Successful')
      },
      (err) => {
        console.log(err)
      }
    )
  }

  cancel() {
    this.cancelRegister.emit(false)
    console.log('canceled')
  }
}
