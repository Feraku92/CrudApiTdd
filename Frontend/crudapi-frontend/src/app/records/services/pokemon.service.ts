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

  getToken(): string | null {
    return localStorage.getItem('token');
  }
  private getAuthHeaders(): Record<string, string> {
  const token = this.getToken();
    return { Authorization: token ? `Bearer ${token}` : '' };
  }

  getAllPokemon(): Observable<Pokemon[]> {
    return this.http.get<Pokemon[]>(`${this.apiPokemonUrl}/getall`, {
      headers: this.getAuthHeaders(),
    });
  }

  searchPokemon(name?: string, number?: number): Observable<Pokemon | null> {
    let params = new HttpParams();
    if (name) params = params.set('name', name);
    if (number != null) params = params.set('number', number.toString());

    return this.http.get<Pokemon | null>(`${this.apiPokemonUrl}/search`, {
      headers: this.getAuthHeaders(),
      params,
    });
  }

  createPokemon(pokedexId: number, name: string, type: string): Observable<Pokemon> {
    return this.http.post<Pokemon>(
      `${this.apiPokemonUrl}/create`,
      { pokedexId, name, type },
      { headers: this.getAuthHeaders() }
    );
  }

  updatePokemon(id: string, updatedPokemon: Pokemon): Observable<Pokemon> {
    return this.http.put<Pokemon>(
      `${this.apiPokemonUrl}/${id}`,
      updatedPokemon,
      { headers: this.getAuthHeaders() }
    );
  }

  deletePokemon(id: string) {
    return this.http.delete<void>(
      `${this.apiPokemonUrl}/${id}`,
      { headers: this.getAuthHeaders() }
    );
  }
}
