import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environment/environmets';
import {Employee} from '../models/employee.model';
@Injectable({
  providedIn: 'root'
})
export class EmployeesService {
  baseApliUrl: string = environment.baseApiUrl;
  constructor(private http:HttpClient) { }

  getAllEmployees():Observable<Employee[]>
  {
    return this.http.get<Employee[]>(this.baseApliUrl + '/api/Employees');
  }

  addEmployee(addEmpLoyeeRequest: Employee):Observable<Employee>{
    addEmpLoyeeRequest.id='000-000-0000';
    return this.http.post<Employee>(this.baseApliUrl + '/api/employees',addEmpLoyeeRequest);
  }

  getEmployee(id:string):Observable<Employee>
  {
    return this.http.get<Employee>(this.baseApliUrl + '/api/employees/'+id);
  }

  updateEmployee(id:string,updateEmpLoyeeRequest:Employee):Observable<Employee>
  {
    return this.http.put<Employee>(this.baseApliUrl + '/api/Employees/'+id,updateEmpLoyeeRequest);
  }

  deleteEmployee(id:string):Observable<Employee>
  {
    return this.http.delete<Employee>(this.baseApliUrl + '/api/Employees/'+id);
 
  }
}
