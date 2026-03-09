using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ApiGenericaCsharp.Servicios.Abstracciones;
using System.Collections.Generic;
using System.Data;

namespace ApiGenericaCsharp.Controllers
{
    [Route("redes")]
    public class RedesController : Controller
    {
        private readonly IServicioCrud _servicioCrud;
        private readonly ILogger<RedesController> _logger;

        public RedesController(IServicioCrud servicioCrud, ILogger<RedesController> logger)
        {
            _servicioCrud = servicioCrud;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(bool success = false)
        {
            try
            {
                _logger.LogInformation("Solicitando listado de redes");
                var redes = await _servicioCrud.ListarAsync("red", "dbo", 1000);

                var notificacionExito = success ? @"
        <div class='alert alert-success alert-dismissible fade show' role='alert'>
            <strong>¡Éxito!</strong> La red ha sido procesada correctamente.
            <button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>
        </div>
" : "";

                var html = @"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Listado de Redes</title>
    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css' rel='stylesheet'
        integrity='sha384-sRIl4kxILFvY47J16cr9ZwB07vP4J8+LH7qKQnuqkuIAvNWLzeN8tE5YBujZqJLB' crossorigin='anonymous'>
    <style>
        .container { margin-top: 30px; }
        .btn-group-top { margin-bottom: 20px; }
    </style>
</head>
<body>
    <nav class='navbar navbar-expand-lg' style='background-color: #e3f2fd;' data-bs-theme='light'>
        <div class='container-fluid'>
            <a class='navbar-brand' href='Index.html'><strong>Sistema Universidad</strong></a>
            <button class='navbar-toggler' type='button' data-bs-toggle='collapse'
                data-bs-target='#navbarSupportedContent' aria-controls='navbarSupportedContent' aria-expanded='false'
                aria-label='Toggle navigation'>
                <span class='navbar-toggler-icon'></span>
            </button>
            <div class='collapse navbar-collapse' id='navbarSupportedContent'>
                <ul class='navbar-nav me-auto mb-2 mb-lg-0'>
                    <li class='nav-item'>
                        <a class='nav-link' href='Index.html'>Inicio</a>
                    </li>
                    <li class='nav-item dropdown'>
                        <a class='nav-link dropdown-toggle' href='#' id='navbarAliados' role='button' data-bs-toggle='dropdown' aria-expanded='false'>
                            Aliados
                        </a>
                        <ul class='dropdown-menu' aria-labelledby='navbarAliados'>
                            <li><a class='dropdown-item' href='form_aliado.html'>Nuevo Aliado</a></li>
                            <li><a class='dropdown-item' href='/aliados'>Listado de Aliados</a></li>
                        </ul>
                    </li>
                    <li class='nav-item dropdown'>
                        <a class='nav-link dropdown-toggle active' href='#' id='navbarRedes' role='button' data-bs-toggle='dropdown' aria-expanded='false'>
                            Redes
                        </a>
                        <ul class='dropdown-menu' aria-labelledby='navbarRedes'>
                            <li><a class='dropdown-item' href='form_red.html'>Nueva Red</a></li>
                            <li><a class='dropdown-item active' href='/redes'>Listado de Redes</a></li>
                        </ul>
                    </li>
                </ul>
                <form class='d-flex' role='search'>
                    <input class='form-control me-2' type='search' placeholder='Buscar' aria-label='Search' />
                    <button class='btn btn-outline-success' type='submit'>Buscar</button>
                </form>
            </div>
        </div>
    </nav>
    <div class='container'>
        <h1 class='mb-4'>Listado de Redes</h1>
        " + notificacionExito + @"
        <div class='btn-group-top'>
            <a href='/form_red.html' class='btn btn-primary'>Agregar Nueva Red</a>
        </div>
";
                if (redes != null && redes.Count > 0)
                {
                    html += @"
        <div class='table-responsive'>
            <table class='table table-striped table-hover'>
                <thead class='table-dark'>
                    <tr>
                        <th>ID</th>
                        <th>Nombre</th>
                        <th>URL</th>
                        <th>País</th>
                        <th>Acciones</th>
                    </tr>
                </thead>
                <tbody>
";
                    foreach (var red in redes)
                    {
                        var idr = red["idr"]?.ToString() ?? "-";
                        var nombre = red["nombre"]?.ToString() ?? "-";
                        var url = red["url"]?.ToString() ?? "-";
                        var pais = red["pais"]?.ToString() ?? "-";
                        var editLink = $"/redes/{idr}/editar";
                        var deleteLink = $"/redes/{idr}/eliminar";
                        string tdActions = $@"
                        <td>
                            <a href='{editLink}' class='btn btn-sm btn-warning'>Editar</a>
                            <form method='post' action='{deleteLink}' style='display:inline-block;'>
                                <button type='submit' class='btn btn-sm btn-danger'>Eliminar</button>
                            </form>
                        </td>";
                        html += @"
                    <tr>
                        <td>" + idr + @"</td>
                        <td>" + nombre + @"</td>
                        <td>" + url + @"</td>
                        <td>" + pais + @"</td>" + tdActions + @"
                    </tr>
";
                    }
                    html += @"
                </tbody>
            </table>
        </div>
        <div class='alert alert-info'>
            Total de registros: " + redes.Count + @"
        </div>
";
                }
                else
                {
                    html += @"
        <div class='alert alert-warning' role='alert'>
            No hay redes registradas. <a href='/form_red.html' class='alert-link'>Agregar la primera red</a>
        </div>
";
                }
                html += @"
    </div>
    <script src='https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js'></script>
</body>
</html>
";
                return Content(html, "text/html");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener listado de redes");
                return Content($@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <title>Error</title>
    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css' rel='stylesheet'>
</head>
<body>
    <div class='container mt-5'>
        <div class='alert alert-danger'>
            <h4>Error al cargar el listado</h4>
            <p>{ex.Message}</p>
            <a href='/redes' class='btn btn-primary'>Volver al listado</a>
        </div>
    </div>
</body>
</html>
", "text/html");
            }
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromForm] IFormCollection formData)
        {
            try
            {
                _logger.LogInformation("Creando nueva red");
                var datos = new Dictionary<string, object?>();
                foreach (var kvp in formData)
                {
                    var valor = formData[kvp.Key].ToString();
                    if (!string.IsNullOrWhiteSpace(valor)) datos[kvp.Key] = valor;
                }
                if (!datos.ContainsKey("idr") || string.IsNullOrWhiteSpace(datos["idr"]?.ToString()))
                {
                    return Content(@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <title>Error</title>
    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css' rel='stylesheet'>
</head>
<body>
    <div class='container mt-5'>
        <div class='alert alert-danger'>
            <h4>Error: El campo ID es obligatorio</h4>
            <a href='/form_red.html' class='btn btn-primary'>Volver al formulario</a>
        </div>
    </div>
</body>
</html>
", "text/html");
                }
                bool creado = await _servicioCrud.CrearAsync("red", "dbo", datos, null);
                if (creado) return Redirect("/redes?success=true");
                return Content(@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <title>Error</title>
    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css' rel='stylesheet'>
</head>
<body>
    <div class='container mt-5'>
        <div class='alert alert-danger'>
            <h4>Error: No se pudo crear el registro</h4>
            <a href='/form_red.html' class='btn btn-primary'>Volver al formulario</a>
        </div>
    </div>
</body>
</html>
", "text/html");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error al crear red");
                return Content($@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <title>Error</title>
    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css' rel='stylesheet'>
</head>
<body>
    <div class='container mt-5'>
        <div class='alert alert-danger'>
            <h4>Error al procesar el registro</h4>
            <p>{ex.Message}</p>
            <a href='/form_red.html' class='btn btn-primary'>Volver al formulario</a>
        </div>
    </div>
</body>
</html>
", "text/html");
            }
        }

        [HttpGet("{idr}/editar")]
        public async Task<IActionResult> Editar(string idr)
        {
            try
            {
                var redes = await _servicioCrud.ObtenerPorClaveAsync("red", "dbo", "idr", idr);
                if (redes == null || redes.Count == 0)
                {
                    return Content(@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <title>Error</title>
    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css' rel='stylesheet'>
</head>
<body>
    <div class='container mt-5'>
        <div class='alert alert-danger'>
            <h4>Error: Red no encontrada</h4>
            <a href='/redes' class='btn btn-primary'>Volver al listado</a>
        </div>
    </div>
</body>
</html>
", "text/html");
                }
                var red = redes[0];
                var nombre = red["nombre"]?.ToString() ?? "";
                var url = red["url"]?.ToString() ?? "";
                var pais = red["pais"]?.ToString() ?? "";
                var html = $@"<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Editar Red</title>
    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css' rel='stylesheet'
        integrity='sha384-sRIl4kxILFvY47J16cr9ZwB07vP4J8+LH7qKQnuqkuIAvNWLzeN8tE5YBujZqJLB' crossorigin='anonymous'>
</head>
<body>
    <div class='container mt-5'>
        <div class='row'>
            <div class='col-md-8 offset-md-2'>
                <h1 class='mb-4'>Editar Red</h1>
                <form action='/redes/{idr}/actualizar' method='post' class='row g-3'>
                    <div class='col-md-6'>
                        <label for='idr' class='form-label'>ID</label>
                        <input type='number' class='form-control' id='idr' name='idr' value='{idr}' disabled>
                    </div>
                    <div class='col-md-6'>
                        <label for='nombre' class='form-label'>Nombre</label>
                        <input type='text' class='form-control' id='nombre' name='nombre' value='{nombre}' maxlength='45' required>
                    </div>
                    <div class='col-md-6'>
                        <label for='url' class='form-label'>URL</label>
                        <input type='text' class='form-control' id='url' name='url' value='{url}' maxlength='45' required>
                    </div>
                    <div class='col-md-6'>
                        <label for='pais' class='form-label'>País</label>
                        <input type='text' class='form-control' id='pais' name='pais' value='{pais}' maxlength='45' required>
                    </div>
                    <div class='col-12'>
                        <button type='submit' class='btn btn-success'>Guardar Cambios</button>
                        <a href='/redes' class='btn btn-secondary'>Cancelar</a>
                    </div>
                </form>
            </div>
        </div>
    </div>
</body>
</html>";
                return Content(html, "text/html");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error al obtener formulario editar");
                return Content("<p>error</p>", "text/html");
            }
        }

        [HttpPost("{idr}/actualizar")]
        public async Task<IActionResult> Actualizar(string idr, [FromForm] IFormCollection formData)
        {
            try
            {
                var datos = new Dictionary<string, object?>();
                foreach(var kvp in formData) { var valor = formData[kvp.Key].ToString(); if(!string.IsNullOrWhiteSpace(valor)) datos[kvp.Key]=valor; }
                int filas = await _servicioCrud.ActualizarAsync("red","dbo","idr",idr,datos,null);
                if(filas>0) return Redirect("/redes?success=true");
                return Content("<p>no actualizó</p>","text/html");
            }
            catch(Exception ex){ _logger.LogError(ex,"error actualizar"); return Content("<p>error</p>","text/html"); }
        }

        [HttpPost("{idr}/eliminar")]
        public async Task<IActionResult> Eliminar(string idr)
        {
            try
            {
                int filas = await _servicioCrud.EliminarAsync("red","dbo","idr",idr);
                if(filas>0) return Redirect("/redes");
                return Content("<p>no eliminado</p>","text/html");
            }
            catch(Exception ex){ _logger.LogError(ex,"error eliminar"); return Content("<p>error</p>","text/html"); }
        }
    }
}