import { Component, OnInit } from "@angular/core";
import { ApiService } from "../services/api.services";

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

}
