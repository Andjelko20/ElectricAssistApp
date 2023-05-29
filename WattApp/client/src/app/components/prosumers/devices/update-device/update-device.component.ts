import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import {  ShowDevices, updateDevices } from 'src/app/models/devices.model';
import { DevicesService } from 'src/app/services/devices.service';
import { JwtToken } from 'src/app/utilities/jwt-token';


@Component({
  selector: 'app-update-device',
  templateUrl: './update-device.component.html',
  styleUrls: ['./update-device.component.css']
})
export class UpdateDeviceComponent implements OnInit{

  @ViewChild('modalContent') modalContent!: TemplateRef<any>;
  @ViewChild('modalContent1') modalContent1!: TemplateRef<any>;
  onClickVisibility!: (this: HTMLElement, ev: MouseEvent) => any;
  offClickVisibility!: (this: HTMLElement, ev: MouseEvent) => any;
  onClickControlability!: (this: HTMLElement, ev: MouseEvent) => any;
  offClickControlability!: (this: HTMLElement, ev: MouseEvent) => any;
  UpdateDevice!: (this: HTMLElement, ev: MouseEvent) => any;

  body: string = ''; 
  btnAction:string='';
  myForm = this.formBuilder.group({
    nameform1: ['', Validators.required],
     nameform2: [''],
     
    
  });
  updateDevice:updateDevices={
    id: 0,
    userId: 0,
    name: '',
    energyInKwh: 0,
    standByKwh: 0,
    visibility: false,
    controlability: false,
    turnOn: false,
    
  }
  previouseName:string=''
  isForm:boolean=false;
  isFormName:boolean=false;
  isFormToggle:boolean=false;
  isFormToggle1:boolean=false;
  isFormToggle2:boolean=false;
  confirmVisibility: boolean = false;
  confirmControlability:boolean=false;
  devices:ShowDevices[] = [];
  idProsumer?:number;
  idDevice?:number;
  constructor(private devicesService:DevicesService,private router:Router,private route:ActivatedRoute
    ,private formBuilder: FormBuilder,private modalService: NgbModal) {
    
   }

  ngOnInit(): void {
    
    let token=new JwtToken();
    this.idProsumer=token.data.id as number;
    this.idDevice=Number(this.route.snapshot.paramMap.get('id'));
  this.devicesService.getDevice( Number(this.route.snapshot.paramMap.get('id')) )
        .subscribe({
          next:(response)=>{
            this.updateDevice={
              id:Number(this.route.snapshot.paramMap.get('id')),
              userId: response.userId,
              name: response.name,
              energyInKwh: response.energyInKwh,
              standByKwh: response.standByKwh,
              visibility: response.visibility,
              controlability: response.controlability,
              turnOn: response.turnOn,
              };
              this.previouseName=this.updateDevice.name
              this.isFormToggle=this.updateDevice.visibility
              this.isFormToggle1=this.updateDevice.controlability
            },
			error:(response)=>{
				this.router.navigate(["/devices"]);
			}
          });
          
   
  }
  formContr()
  {
    if(this.previouseName!=this.updateDevice.name)
    {
      this.isFormName=true
    }
    else if(this.previouseName===this.updateDevice.name)
    {
      this.isFormName=false
    }
    
  }
  formContr1(){
    
    if(this.isFormToggle===this.updateDevice.visibility && this.isFormToggle1===this.updateDevice.controlability)
    {
        this.isFormToggle2=false
    }
    else 
    {
      
      this.isFormToggle2=true
    }
  }
  upDate()
  {
    
    this.modalService.open(this.modalContent);
    const popup= document.getElementById('popup');
    if(popup!=null)
    {
      this.body="Do you want to update this device?"
    this.btnAction="Update";
        popup.removeEventListener('click', this.UpdateDevice);
        this.UpdateDevice=()=> {
          this.devicesService.upDateDevice(this.updateDevice)
          .subscribe({
            next:()=>{
              this.router.navigate(['/device/'+Number(this.route.snapshot.paramMap.get('id'))]);
            }
          });
        popup.removeEventListener('click',this.UpdateDevice);
      };
      popup.addEventListener('click',this.UpdateDevice);
    }
  this.isFormName=false
   this.isFormToggle2=false
  }
 
  controlabilityOn(){
    if( this.updateDevice.visibility===false)
    {
      this.modalService.open(this.modalContent1);
    }
    else
    {

      
      this.modalService.open(this.modalContent);
      const controlabilityOnPopup= document.getElementById('popup');
    
      this.confirmControlability=false;
      if(controlabilityOnPopup!=null)
      {this.body="Do you want to turn on controlability?"
      this.btnAction="Turn On";
        controlabilityOnPopup.removeEventListener('click', this.onClickControlability);
        this.onClickControlability=()=> {
            this.updateDevice.controlability=true
            this.confirmControlability=true;
            this.formContr1()
            controlabilityOnPopup.removeEventListener('click',this.onClickControlability);
      };
      controlabilityOnPopup.addEventListener('click',this.onClickControlability);
      }
  }
    
  }
  controlabilityOff(){
    this.modalService.open(this.modalContent);
    const controlabilityOffPopup= document.getElementById('popup');
    
    this.confirmControlability=false;
    if(controlabilityOffPopup!=null)
    {
      this.body="Do you want to turn off controlability?"
      this.btnAction="Turn Off";
      controlabilityOffPopup.removeEventListener('click', this.offClickControlability);
      this.offClickControlability=()=> {
          this.updateDevice.controlability=false;
          this.confirmControlability=true;
          this.formContr1()
          controlabilityOffPopup.removeEventListener('click',this.offClickControlability);
      };
      controlabilityOffPopup.addEventListener('click',this.offClickControlability);
    }
  }

  visibilityOn()
  {
    
    this.modalService.open(this.modalContent);
    const visibilityOnPopup= document.getElementById('popup');
 
    this.confirmVisibility=false;
    if(visibilityOnPopup!=null)
    {this.body="Do you want to turn on visibility?"
    this.btnAction="Turn On";
      visibilityOnPopup.removeEventListener('click', this.onClickVisibility);
      this.onClickVisibility=()=> {
        
          this.updateDevice.visibility=true
          this.confirmVisibility=true;

          this.formContr1()
          visibilityOnPopup.removeEventListener('click',this.onClickVisibility);
     };
     visibilityOnPopup.addEventListener('click',this.onClickVisibility);
    
    }
    
    
    
  }
  visibilityOff()
  {
    this.modalService.open(this.modalContent);
    
    const visibilityOffPopup= document.getElementById('popup');
    this.confirmVisibility=false;
    if(visibilityOffPopup!=null)
    {
      this.body="Do you want to turn off visibility?"
      this.btnAction="Turn Off";
      visibilityOffPopup.removeEventListener('click', this.offClickVisibility);
      this.offClickVisibility=()=> {
        
          this.updateDevice.visibility=false;
          this.updateDevice.controlability=false;
          this.confirmVisibility=true;
          this.formContr1()
          visibilityOffPopup.removeEventListener('click',this.offClickVisibility);
     };
     visibilityOffPopup.addEventListener('click',this.offClickVisibility);
    
    }
  }

}
