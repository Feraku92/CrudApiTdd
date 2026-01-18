import { Injectable } from '@angular/core';
import { Pokemon } from '../models/pokemon.model';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams  } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class PokemonService {
  private readonly apiPokemonUrl = `${environment.apiUrl}/Pokemon`;
  
  constructor(private http: HttpClient) {}

  getAllPokemon(): Observable<Pokemon[]> {
    return this.http.get<Pokemon[]>(`${this.apiPokemonUrl}/getall`);
  }

  searchPokemon(name?: string, number?: number): Observable<Pokemon | null> {
    let params = new HttpParams();
    if (name) params = params.set('name', name);
    if (number != null) params = params.set('number', number.toString());

    return this.http.get<Pokemon | null>(`${this.apiPokemonUrl}/search`, { params });
  }

  createPokemon(pokedexId: number, name: string, type: string): Observable<Pokemon> {
    return this.http.post<Pokemon>(`${this.apiPokemonUrl}/create`, { pokedexId, name, type }
    );
  }

  updatePokemon(id: string, updatedPokemon: Pokemon): Observable<Pokemon> {
    return this.http.put<Pokemon>(`${this.apiPokemonUrl}/${id}`, updatedPokemon);
  }

  deletePokemon(id: string) {
    return this.http.delete<void>(`${this.apiPokemonUrl}/${id}`);
  }
}
