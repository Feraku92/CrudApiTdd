import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecordComponent } from './record.component';

describe('Record.coponent', () => {
  let component: RecordComponent;
  let fixture: ComponentFixture<RecordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecordComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecordComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
