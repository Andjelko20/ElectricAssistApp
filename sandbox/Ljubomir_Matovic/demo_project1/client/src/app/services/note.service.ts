import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environment/environment';
import { Note } from '../models/note.model';

@Injectable({
  providedIn: 'root'
})
export class NoteService {

  apiUrl:string;

  constructor(private http:HttpClient) { 
    this.apiUrl=environment.baseUrl+"/api/notes";
  }

  getAllNotes(){
    return this.http.get<Note[]>(this.apiUrl);
  }

  addNote(note:Note){
    note.id='00000000-0000-0000-0000-000000000000';
    return this.http.post(this.apiUrl,note);
  }

  updateNote(note:Note){
    return this.http.put(this.apiUrl,note);
  }

  deleteNote(id:string){
    return this.http.delete(this.apiUrl+"/"+id);
  }
}
