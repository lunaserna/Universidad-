using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ApiGenericaCsharp.Servicios.Abstracciones;
using System.Collections.Generic;
using System.Data;

namespace ApiGenericaCsharp.Controllers
{
    [Route("aliados")]
    public class AliadosController : Controller
    {
        private readonly IServicioCrud _servicioCrud;
        private readonly ILogger<AliadosController> _logger;

        public AliadosController(IServicioCrud servicioCrud, ILogger<AliadosController> logger)
        {
            _servicioCrud = servicioCrud;
            _logger = logger;
        }

        /// <summary>
        /// GET /aliados - Muestra el listado de todos los aliados
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(bool success = false)
        {
            try
            {
                _logger.LogInformation("Solicitando listado de aliados");
                
                // Obtener todos los aliados de la tabla
                var aliados = await _servicioCrud.ListarAsync("aliado", "dbo", 1000);
                
                var notificacionExito = success ? @"
        <div class='alert alert-success alert-dismissible fade show' role='alert'>
            <strong>¡Éxito!</strong> El aliado ha sido registrado correctamente.
            <button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>
        </div>
" : "";
                
                var notificacionActualizado = success ? @"
        <div class='alert alert-success alert-dismissible fade show' role='alert'>
            <strong>¡Éxito!</strong> El aliado ha sido actualizado correctamente.
            <button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>
        </div>
" : "";
                
                var html = @"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Listado de Aliados</title>
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
                        <a class='nav-link dropdown-toggle active' href='#' id='navbarAliados' role='button' data-bs-toggle='dropdown' aria-expanded='false'>
                            Aliados
                        </a>
                        <ul class='dropdown-menu' aria-labelledby='navbarAliados'>
                            <li><a class='dropdown-item' href='form_aliado.html'>Nuevo Aliado</a></li>
                            <li><a class='dropdown-item active' href='/aliados'>Listado de Aliados</a></li>
                        </ul>
                    </li>
                    <li class='nav-item dropdown'>
                        <a class='nav-link dropdown-toggle' href='#' id='navbarRedes' role='button' data-bs-toggle='dropdown' aria-expanded='false'>
                            Redes
                        </a>
                        <ul class='dropdown-menu' aria-labelledby='navbarRedes'>
                            <li><a class='dropdown-item' href='form_red.html'>Nueva Red</a></li>
                            <li><a class='dropdown-item' href='/redes'>Listado de Redes</a></li>
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
        <h1 class='mb-4'>Listado de Aliados</h1>
        
        " + notificacionExito + @"
        
        <div class='btn-group-top'>
            <a href='/form_aliado.html' class='btn btn-primary'>Agregar Nuevo Aliado</a>
        </div>
";
                
                if (aliados != null && aliados.Count > 0)
                {
                    html += @"
        <div class='table-responsive'>
            <table class='table table-striped table-hover'>
                <thead class='table-dark'>
                    <tr>
                        <th>NIT</th>
                        <th>Razón Social</th>
                        <th>Nombre Contacto</th>
                        <th>Correo</th>
                        <th>Teléfono</th>
                        <th>Ciudad</th>
                        <th>Acciones</th>
                    
                    </tr>
                </thead>
                <tbody>
";
                    
                    foreach (var aliado in aliados)
                    {
                        var nit = aliado["nit"]?.ToString() ?? "-";
                        var razonSocial = aliado["razon_social"]?.ToString() ?? "-";
                        var nombreContacto = aliado["nombre_contacto"]?.ToString() ?? "-";
                        var correo = aliado["correo"]?.ToString() ?? "-";
                        var telefono = aliado["telefono"]?.ToString() ?? "-";
                        var ciudad = aliado["ciudad"]?.ToString() ?? "-";
                        
                        var editLink = $"/aliados/{nit}/editar";
                        var deleteLink = $"/aliados/{nit}/eliminar";
                        
                        string tdActions = $@"
                        <td>
                            <a href='{editLink}' class='btn btn-sm btn-warning'>Editar</a>
                            <form method='post' action='{deleteLink}' style='display:inline-block;'>
                                <button type='submit' class='btn btn-sm btn-danger'>Eliminar</button>
                            </form>
                        </td>";
                        
                        html += @"
                    <tr>
                        <td>" + nit + @"</td>
                        <td>" + razonSocial + @"</td>
                        <td>" + nombreContacto + @"</td>
                        <td>" + correo + @"</td>
                        <td>" + telefono + @"</td>
                        <td>" + ciudad + @"</td>" + tdActions + @"
                    </tr>
";
                    }
                    
                    html += @"
                </tbody>
            </table>
        </div>
        <div class='alert alert-info'>
            Total de registros: " + aliados.Count + @"
        </div>
";
                }
                else
                {
                    html += @"
        <div class='alert alert-warning' role='alert'>
            No hay aliados registrados. <a href='/form_aliado.html' class='alert-link'>Agregar el primer aliado</a>
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
                _logger.LogError(ex, "Error al obtener listado de aliados");
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
            <a href='/aliados' class='btn btn-primary'>Volver al listado</a>
        </div>
    </div>
</body>
</html>
", "text/html");
            }
        }

        /// <summary>
        /// POST /aliados/crear - Procesa el formulario de creación y redirige al listado
        /// </summary>
        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromForm] IFormCollection formData)
        {
            try
            {
                _logger.LogInformation("Creando nuevo aliado desde formulario HTML");
                
                // Convertir IFormCollection a Dictionary
                var datos = new Dictionary<string, object?>();
                foreach (var kvp in formData)
                {
                    var valor = formData[kvp.Key].ToString();
                    if (!string.IsNullOrWhiteSpace(valor))
                    {
                        datos[kvp.Key] = valor;
                    }
                }
                
                // Validar campos requeridos
                if (!datos.ContainsKey("nit") || string.IsNullOrWhiteSpace(datos["nit"]?.ToString()))
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
            <h4>Error: El campo NIT es obligatorio</h4>
            <a href='/form_aliado.html' class='btn btn-primary'>Volver al formulario</a>
        </div>
    </div>
</body>
</html>
", "text/html");
                }
                
                // Insertar en base de datos usando el servicio
                bool creado = await _servicioCrud.CrearAsync("aliado", "dbo", datos, null);
                
                if (creado)
                {
                    _logger.LogInformation("Aliado creado exitosamente, redirigiendo al listado");
                    
                    // Redirigir al listado con parámetro de éxito
                    return Redirect("/aliados?success=true");
                }
                else
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
            <h4>Error: No se pudo crear el registro</h4>
            <a href='/form_aliado.html' class='btn btn-primary'>Volver al formulario</a>
        </div>
    </div>
</body>
</html>
", "text/html");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear aliado");
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
            <a href='/form_aliado.html' class='btn btn-primary'>Volver al formulario</a>
        </div>
    </div>
</body>
</html>
", "text/html");
            }
        }

        /// <summary>
        /// GET /aliados/{nit}/editar - Muestra el formulario de edición
        /// </summary>
        [HttpGet("{nit}/editar")]
        public async Task<IActionResult> Editar(string nit)
        {
            try
            {
                _logger.LogInformation("Solicitando formulario de edición para aliado: {NIT}", nit);
                
                // Obtener el aliado actual
                var aliados = await _servicioCrud.ObtenerPorClaveAsync("aliado", "dbo", "nit", nit);
                
                if (aliados == null || aliados.Count == 0)
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
            <h4>Error: Aliado no encontrado</h4>
            <a href='/aliados' class='btn btn-primary'>Volver al listado</a>
        </div>
    </div>
</body>
</html>
", "text/html");
                }
                
                var aliado = aliados[0];
                var nit_val = aliado["nit"]?.ToString() ?? "";
                var razonSocial = aliado["razon_social"]?.ToString() ?? "";
                var nombreContacto = aliado["nombre_contacto"]?.ToString() ?? "";
                var correo = aliado["correo"]?.ToString() ?? "";
                var telefono = aliado["telefono"]?.ToString() ?? "";
                var ciudad = aliado["ciudad"]?.ToString() ?? "";
                
                var html = $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Editar Aliado</title>
    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css' rel='stylesheet'
        integrity='sha384-sRIl4kxILFvY47J16cr9ZwB07vP4J8+LH7qKQnuqkuIAvNWLzeN8tE5YBujZqJLB' crossorigin='anonymous'>
</head>
<body>
    <div class='container mt-5'>
        <div class='row'>
            <div class='col-md-8 offset-md-2'>
                <h1 class='mb-4'>Editar Aliado</h1>
                <form action='/aliados/{nit}/actualizar' method='post' class='row g-3'>
                    <div class='col-md-6'>
                        <label for='nit' class='form-label'>NIT</label>
                        <input type='number' class='form-control' id='nit' name='nit' value='{nit_val}' disabled>
                    </div>
                    <div class='col-md-6'>
                        <label for='razon_social' class='form-label'>Razón Social</label>
                        <input type='text' class='form-control' id='razon_social' name='razon_social' value='{razonSocial}' maxlength='60' required>
                    </div>
                    <div class='col-md-6'>
                        <label for='nombre_contacto' class='form-label'>Nombre de Contacto</label>
                        <input type='text' class='form-control' id='nombre_contacto' name='nombre_contacto' value='{nombreContacto}' maxlength='60' required>
                    </div>
                    <div class='col-md-6'>
                        <label for='correo' class='form-label'>Correo</label>
                        <input type='email' class='form-control' id='correo' name='correo' value='{correo}' maxlength='70' required>
                    </div>
                    <div class='col-md-6'>
                        <label for='telefono' class='form-label'>Teléfono</label>
                        <input type='text' class='form-control' id='telefono' name='telefono' value='{telefono}' maxlength='45' required>
                    </div>
                    <div class='col-md-6'>
                        <label for='ciudad' class='form-label'>Ciudad</label>
                        <input type='text' class='form-control' id='ciudad' name='ciudad' value='{ciudad}' maxlength='45' required>
                    </div>
                    <div class='col-12'>
                        <button type='submit' class='btn btn-success'>Guardar Cambios</button>
                        <a href='/aliados' class='btn btn-secondary'>Cancelar</a>
                    </div>
                </form>
            </div>
        </div>
    </div>
</body>
</html>
";
                
                return Content(html, "text/html");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener formulario de edición para NIT: {NIT}", nit);
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
            <h4>Error al cargar el formulario de edición</h4>
            <p>{ex.Message}</p>
            <a href='/aliados' class='btn btn-primary'>Volver al listado</a>
        </div>
    </div>
</body>
</html>
", "text/html");
            }
        }

        /// <summary>
        /// POST /aliados/{nit}/actualizar - Procesa la actualización
        /// </summary>
        [HttpPost("{nit}/actualizar")]
        public async Task<IActionResult> Actualizar(string nit, [FromForm] IFormCollection formData)
        {
            try
            {
                _logger.LogInformation("Actualizando aliado con NIT: {NIT}", nit);
                
                // Convertir IFormCollection a Dictionary
                var datos = new Dictionary<string, object?>();
                foreach (var kvp in formData)
                {
                    var valor = formData[kvp.Key].ToString();
                    if (!string.IsNullOrWhiteSpace(valor))
                    {
                        datos[kvp.Key] = valor;
                    }
                }
                
                // Actualizar en base de datos usando el servicio
                int filasAfectadas = await _servicioCrud.ActualizarAsync("aliado", "dbo", "nit", nit, datos, null);
                
                if (filasAfectadas > 0)
                {
                    _logger.LogInformation("Aliado actualizado exitosamente, NIT: {NIT}", nit);
                    return Redirect("/aliados?success=true");
                }
                else
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
            <h4>Error: No se pudo actualizar el registro</h4>
            <a href='/aliados' class='btn btn-primary'>Volver al listado</a>
        </div>
    </div>
</body>
</html>
", "text/html");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar aliado con NIT: {NIT}", nit);
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
            <h4>Error al procesar la actualización</h4>
            <p>{ex.Message}</p>
            <a href='/aliados' class='btn btn-primary'>Volver al listado</a>
        </div>
    </div>
</body>
</html>
", "text/html");
            }
        }

        /// <summary>
        /// POST /aliados/{nit}/eliminar - Elimina un aliado
        /// </summary>
        [HttpPost("{nit}/eliminar")]
        public async Task<IActionResult> Eliminar(string nit)
        {
            try
            {
                _logger.LogInformation("Eliminando aliado con NIT: {NIT}", nit);
                
                // Eliminar de la base de datos usando el servicio
                int filasEliminadas = await _servicioCrud.EliminarAsync("aliado", "dbo", "nit", nit);
                
                if (filasEliminadas > 0)
                {
                    _logger.LogInformation("Aliado eliminado exitosamente, NIT: {NIT}", nit);
                    return Redirect("/aliados");
                }
                else
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
            <h4>Error: No se pudo eliminar el registro</h4>
            <a href='/aliados' class='btn btn-primary'>Volver al listado</a>
        </div>
    </div>
</body>
</html>
", "text/html");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar aliado con NIT: {NIT}", nit);
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
            <h4>Error al procesar la eliminación</h4>
            <p>{ex.Message}</p>
            <a href='/aliados' class='btn btn-primary'>Volver al listado</a>
        </div>
    </div>
</body>
</html>
", "text/html");
            }
        }
    }
}
