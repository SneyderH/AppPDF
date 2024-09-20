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

  FillData() {
    this.apiService.getData().subscribe(data => {
      this.data = data;
      console.log(this.data)
    })
  }

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
          console.log(result);
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

}
