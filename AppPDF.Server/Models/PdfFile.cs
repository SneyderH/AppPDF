using System;
using System.Collections.Generic;

namespace AppPDF.Server.Models;

public partial class PdfFile
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Archivo { get; set; }

    public IFormFile FilePDF { get; set; }
}
