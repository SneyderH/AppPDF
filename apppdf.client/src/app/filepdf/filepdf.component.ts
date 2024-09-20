import { Component, OnInit } from "@angular/core";
import { ApiService } from "../services/api.services";
import { HttpErrorResponse, HttpResponse } from "@angular/common/http";

@Component({
  selector: 'app-pdf',
  templateUrl: './filepdf.component.html',
  styleUrls: ['./filepdf.component.css']
})
export class PDFComponent implements OnInit {


  data: any[] = [];
  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.FillData();
  }

  //INSERTAR DOCUMENTO PDF
  FillData() {
    this.apiService.getData().subscribe(data => {
      this.data = data;
      //this.FillData();
    })
  }


  //LISTAR DOCUMENTOS INSERTADOS
  getFilePDF(event: Event, fileInput: HTMLInputElement) {

    event.preventDefault();
    const files: FileList | null = fileInput.files;

    if (files!.length > 0 && files != null) {
      const formData = new FormData();

      Array.prototype.forEach.call(files, (file: File) => {
        formData.append("File", file)
      });

      this.apiService.Upload(formData).subscribe({
        next: (result: HttpResponse<FileList>) => {
          this.FillData();
        }, error: (Error: HttpErrorResponse) => {
          console.log(Error);
        }, complete: () => {
          console.log("Archivo insertado.");
        }
      });
    }
    else {
      alert("No hay archivos cargados.")
    }
  }

  //ELIMINAR DOCUMENTOS CARGADOS
  delete(id: string): void {
    this.apiService.DeleteFile(id).subscribe({
      next: () => {
        console.log("Archivo eliminado.");
        this.FillData(); // Actualiza la lista después de eliminar
      }, error: (error: HttpErrorResponse) => {
        console.error("Error al eliminar el archivo:", error);
      }
    });
  }

  //VISUALIZAR PDF EN UNA PESTAÑA NUEVA
  viewPdf(id: string): void {
    this.apiService.getPdfFile(id).subscribe({
      next: (response: Blob) => {
        const fileURL = URL.createObjectURL(response);
        window.open(fileURL, '_blank');
      },error: (error: HttpErrorResponse) => {
        console.error("Error al visualizar el archivo:", error);
      }
    });
  }

  //DESCARGAR PDF CON FIRMA
  downloadPdf(id: string) {
    this.apiService.downloadPdf(id).subscribe((response: Blob) => {
      const url = window.URL.createObjectURL(response);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'Archivo_Firmado.pdf';
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    }, error => {
      console.error('Error al descargar el archivo', error);
    });
  }

}
