import { Component, OnInit } from '@angular/core';
import { Note } from './models/note.model';
import { NoteService } from './services/note.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'client';
  notes:Note[]=[];
  note:Note={
    id:'',
    title:'',
    description:''
  };
  constructor(private noteService:NoteService){}

  ngOnInit(): void {
      this.getAllNotes();
  }

  getAllNotes(){
    this.noteService.getAllNotes().subscribe(data=>{
      this.notes=data;
      this.note={
        id:'',
        title:'',
        description:''
      };
    });
  }
  deleteNote(id:string){
    this.noteService.deleteNote(id).subscribe(data=>{
      this.getAllNotes();
    });
  }

  onSubmit(){
    if(this.note.title==='' || this.note.description===''){
      alert("Naslov i opis su obavezni");
      return;
    }
    if(this.note.id==='')
      this.noteService.addNote(this.note).subscribe(data=>{
        this.getAllNotes();
      });
    else this.noteService.updateNote(this.note).subscribe(data=>{
      this.getAllNotes();
    });
    this.note={
      id:'',
      title:'',
      description:''
    };
  }

  populateForm(note:Note){
    this.note=note;
  }
}
