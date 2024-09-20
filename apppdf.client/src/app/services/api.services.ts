import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class ApiService {

  private urlApi = 'https://localhost:7102/api';

  constructor(private http: HttpClient) { }

  public getData(): Observable<any> {
    return this.http.get<any>(`${ this.urlApi }/PdfFile/GetFiles`);
  }

  Upload(formData: FormData): Observable<any> {
    return this.http.post<FormData>(`${ this.urlApi }/PdfFile/Subir`, formData);
  }

  DeleteFile(id: string): Observable<void> {
    return this.http.delete<void>(`${ this.urlApi }/PdfFile/DeleteFiles/${id}`)
  }

  getPdfFile(id: string) {
    return this.http.get(`${this.urlApi}/PdfFile/ShowPDF/${id}`, { responseType: 'blob' })
  }
}
