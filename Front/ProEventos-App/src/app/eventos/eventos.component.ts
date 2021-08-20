import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos: any = [];
  public eventosFiltrados: any = [];
  public larguraImagem: number = 150;
  public margemImagem: number = 2;
  public exibirImagem = true;
  private _filtroLista : string = '';

  public get filtroLista() {
    return this._filtroLista;
  }

  public set filtroLista(value: string){
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  filtrarEventos(filstrarPor: string): any {
    filstrarPor = filstrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: any) => evento.tema.toLocaleLowerCase().indexOf(filstrarPor) !== -1 ||
      evento.local.toLocaleLowerCase().indexOf(filstrarPor) !== -1
    )
  }

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getEventos();
  }

  public alterarImagem() {
    this.exibirImagem = !this.exibirImagem;
  }

  public getEventos(): void {
    this.http.get('https://localhost:5001/api/eventos').subscribe(
      Response => {
        this.eventos = Response;
        this.eventosFiltrados = this.eventos;
      },
      error => console.log(error)
    );
  }

}
