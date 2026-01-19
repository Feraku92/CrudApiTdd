import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { PokemonService } from '../../services/pokemon.service';
import { Pokemon } from '../../models/pokemon.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../auth/services/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-record',
  imports: [CommonModule, FormsModule],
  templateUrl: './record.component.html',
  styleUrl: './record.component.css',
})
export class RecordComponent implements OnInit {
  loading: boolean = false;
  pokemons: Pokemon[] = [];
  searchName: string = '';
  searchNumber: number | null = null;
  foundPokemon: Pokemon | null = null;
  newPokedexId: number | null = null;
  newName: string = '';
  newType: string = '';

  updateId: string = '';
  updatePokedexId: number | null = null;
  updateName: string = '';
  updateType: string = '';

  deleteId: number | null = null;
  searchError: string = '';
  createError: string = '';
  updateError: string = '';
  deleteError: string = '';
  createMessage: string = '';
  updateMessage: string = '';
  deleteMessage: string = '';

    constructor(
    private pokemonService: PokemonService, 
    private cd: ChangeDetectorRef,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadAllPokemons();
  }

  loadAllPokemons(): void {
    this.loading = true;
    this.pokemonService.getAllPokemon().subscribe({
      next: (data) => {
        this.pokemons = data ?? [];
        this.loading = false;
        this.cd.detectChanges();
      },
      error: (err) => {
        this.pokemons = [];
        this.loading = false;
        this.cd.detectChanges();
      }
    });
  }

  searchPokemon() {
    this.searchError = '';
    this.foundPokemon = null;
    if (!this.searchName && !this.searchNumber) {
      this.foundPokemon = null;
      this.searchError = 'Enter name or number';
      return;
    }

    this.pokemonService.searchPokemon(this.searchName, this.searchNumber ?? 0).subscribe({
      next: (pokemon) => {
        if (pokemon) {
          this.foundPokemon = pokemon;
          this.searchError = '';

          confirm(
            `Pokémon found:\n\n` +
            `Name: ${pokemon.name}\n` +
            `Pokédex: ${pokemon.pokedexId}\n` +
            `Type: ${pokemon.type}\n\n`
          );

        } else {
          this.foundPokemon = null;
          this.searchError = 'The Pokémon was not found';
        }
        this.cd.detectChanges();
      },
      error: (err) => {
        this.foundPokemon = null;
        this.searchError = err.error.message;
        console.log(this.searchError);
        this.cd.detectChanges();
      }
    });
  }


  createPokemon() {
    this.createMessage = '';
    this.createError = '';
    if (this.newPokedexId === null) {
      this.createError = 'You must enter a Pokédex number';
      return;
    }
    
    if (!this.newName || this.newName.trim() === '') {
      this.createError = 'You must enter a Pokémon name';
      return;
    }
    
    if (!this.newType || this.newType.trim() === '') {
      this.createError = 'You must enter a Pokémon type';
      return;
    }

    this.pokemonService.createPokemon(this.newPokedexId, this.newName, this.newType).subscribe({
      next: _ => {
        this.createMessage = 'Successfully created Pokémon!';
        this.createError = '';
        this.clearCreateForm();
        this.loadAllPokemons();
      },
      error: (err) => {
        this.createError = err.error.error;
        this.createMessage = '';
        this.cd.detectChanges();
      }
    });
  }

  clearCreateForm() {
    this.newPokedexId = null;
    this.newName = '';
    this.newType = '';
  }

  updatePokemon() {
    this.updateMessage = '';
    this.updateError = '';
    if (this.updatePokedexId === null) {
      this.updateError = 'You must enter a Pokédex number';
      return;
    }

    if (!this.updateName || this.updateName.trim() === '') {
      this.updateError = 'You must enter a Pokémon name';
      return;
    }

    if (!this.updateType || this.updateType.trim() === '') {
      this.updateError = 'You must enter a Pokémon type';
      return;
    }
    const updated: Partial<Pokemon> = {};
    if (this.updatePokedexId) updated.pokedexId = this.updatePokedexId;
    if (this.updateName) updated.name = this.updateName;
    if (this.updateType) updated.type = this.updateType;

    const pokemonToUpdate = this.pokemons.find(p => p.pokedexId === this.updatePokedexId);

    if (!pokemonToUpdate) {
      this.updateError = `No Pokémon found with Pokédex ${this.updatePokedexId}`;
      return;
    }

    this.pokemonService.updatePokemon(pokemonToUpdate.id, updated as Pokemon).subscribe({
      next: () => {
        this.updateMessage = 'Pokémon updated!';
        this.updateError = '';
        this.clearUpdateForm();
        this.loadAllPokemons();
      },
      error: (err) => {
        this.updateError = err.error.error;
        this.updateMessage = '';
        this.cd.detectChanges();
      }
    });
  }

  clearUpdateForm() {
    this.updateId = '';
    this.updatePokedexId = null;
    this.updateName = '';
    this.updateType = '';
  }

  deletePokemon() {
    this.deleteMessage = '';
    this.deleteError = '';
    if (this.deleteId === null) {
      this.deleteError = 'You must enter a Pokédex number';
      return;
    }
    const pokemonToDelete = this.pokemons.find(p => p.pokedexId === this.deleteId);

    if (!pokemonToDelete) {
      this.deleteError = `No Pokémon found with Pokédex ${this.deleteId}`;
      return;
    }

    this.pokemonService.deletePokemon(pokemonToDelete.id).subscribe({
      next: _ => {
        this.deleteMessage = 'Pokémon deleted!';
        this.deleteError = '';
        this.deleteId = null;
        this.loadAllPokemons();
      },
      error: (err) =>{
          this.deleteError = err.error.error;
          this.deleteMessage = '';
          this.cd.detectChanges();
      }
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
