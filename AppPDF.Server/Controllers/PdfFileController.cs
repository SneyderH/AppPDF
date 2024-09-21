using AppPDF.Server.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace AppPDF.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfFileController : ControllerBase
    {
        private readonly string _pdfFilePath;
        private readonly string _cadenaSQL;

        public PdfFileController(IConfiguration config)
        {
            _pdfFilePath = config.GetSection("Configuration").GetSection("ServerPath").Value;
            _cadenaSQL = config.GetConnectionString("cadenaSQL");
        }

        [HttpPost]
        [Route("Subir")]
        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
        public IActionResult Subir([FromForm]IFormFile File)
        {
            string documentPath = Path.Combine(_pdfFilePath, File.FileName);
            string ext = Path.GetExtension(documentPath);

            if (ext.ToLower() == ".pdf")
            {
                try
                {
                    using (var memorystream = new MemoryStream())
                    {
                        File.CopyTo(memorystream);
                        var fileBytes = memorystream.ToArray();
                        var fileData = Convert.ToBase64String(fileBytes);

                        using (var conexion = new SqlConnection(_cadenaSQL))
                        {
                            conexion.Open();
                            var cmd = new SqlCommand("SP_SAVE_FILE", conexion);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@nombre", SqlDbType.VarChar).Value = File.FileName;
                            cmd.Parameters.AddWithValue("@archivo", SqlDbType.VarChar).Value = fileData;
                            cmd.ExecuteNonQuery();
                        }
                    }


                    return StatusCode(StatusCodes.Status200OK, new { message = "Archivo Guardado." });

                }
                catch (Exception ex)
                {

                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "Archivo no guardado, solo se permiten archivos .pdf" });
            }

        }


        [HttpGet("GetFiles")]
        public IActionResult GetFiles()
        {
            try
            {
                using (var conexion = new SqlConnection(_cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_LIST_FILE", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    using (var reader = cmd.ExecuteReader())
                    {
                        var files = new List<PdfFile>();

                        while (reader.Read())
                        {
                            files.Add(new PdfFile
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Archivo = reader["archivo"].ToString()
                            });
                        }


                        return Ok(files);
                    }

                    //return StatusCode(StatusCodes.Status200OK, new { message = "Archivo Listado." });
                }

            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpDelete("DeleteFiles/{id}")]
        public IActionResult DeleteFile(string id)
        {

            string idFile = string.Empty;
            try
            {
                using(var conexion = new SqlConnection(_cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_DELETE_FILE", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(id));
                    cmd.ExecuteNonQuery();

                    idFile = id;
                }

                return StatusCode(StatusCodes.Status200OK, new { message = "Archivo Eliminado." });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("ShowPDF/{id}")]
        public IActionResult ShowPDF(string id)
        {
            try
            {
                using (var conexion = new SqlConnection(_cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_GET_FILE", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(id));

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        var fileContent = reader["archivo"].ToString();
                        var fileBytes = Convert.FromBase64String(fileContent);
                        return File(fileBytes, "application/pdf", "file.pdf");
                    }
                    else
                    {
                        return NotFound(new { message = "Archivo no encontrado." });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost("sign/{id}")]
        public IActionResult SignPdf(string id)
        {
            // Obtener el PDF desde la base de datos usando el id
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                var cmd = new SqlCommand("SP_GET_FILE", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(id));

                using(var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var fileContent = reader["archivo"].ToString();
                        var fileBytes = Convert.FromBase64String(fileContent);
                        return File(fileBytes, "application/pdf", "Archivo_Firmado.pdf");
                    }
                    else
                    {
                        return NotFound("Archivo no encontrado.");
                    }
                }
            }
        }

    }
}
