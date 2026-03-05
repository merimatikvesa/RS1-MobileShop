import { Component,Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-image-preview-dialog',
  imports: [CommonModule, MatDialogModule],
  templateUrl: './image-preview-dialog.component.html',
  styleUrl: './image-preview-dialog.component.css'
})
export class ImagePreviewDialogComponent {
  zoom = 1;
  posX = 0;
  posY = 0;

  dragging = false;
  startX = 0;
  startY = 0;

  constructor(@Inject(MAT_DIALOG_DATA) public data: { src: string }) {}

  onWheel(event: WheelEvent) {
    event.preventDefault();

    if (event.deltaY < 0) {
      this.zoom = Math.min(this.zoom + 0.2, 4);
    } else {
      this.zoom = Math.max(this.zoom - 0.2, 1);
    }
    if(this.zoom <= 1){
      this.zoom=1;
      this.posX=0;
      this.posY=0;
      this.dragging=false;
    }
    
  }

  startDrag(event: MouseEvent) {
    if (this.zoom <= 1) return;

    this.dragging = true;
    this.startX = event.clientX - this.posX;
    this.startY = event.clientY - this.posY;
  }

  drag(event: MouseEvent) {
    if (!this.dragging) return;

    this.posX = event.clientX - this.startX;
    this.posY = event.clientY - this.startY;
  }

  stopDrag() {
    this.dragging = false;
  }

  resetZoom() {
    this.zoom = 1;
    this.posX = 0;
    this.posY = 0;
  }
}
