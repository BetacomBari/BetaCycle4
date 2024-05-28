import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecommandationsComponent } from './recommandations.component';

describe('RecommandationsComponent', () => {
  let component: RecommandationsComponent;
  let fixture: ComponentFixture<RecommandationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecommandationsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RecommandationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
