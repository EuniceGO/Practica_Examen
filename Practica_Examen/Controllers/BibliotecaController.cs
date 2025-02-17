using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica_Examen.Models;

namespace Practica_Examen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BibliotecaController : ControllerBase
    {
        private readonly BibliotecaContext _bibliotecaContext;

        public BibliotecaController(BibliotecaContext bibliotecaContext)
        {
            _bibliotecaContext = bibliotecaContext;
        }

        [HttpGet]
        [Route("GetAutor")]

        public IActionResult Get()
        {
            var listadoAutor = (from a in _bibliotecaContext.Autor
                                select a).ToList();

            if (listadoAutor.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoAutor);
        }
        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            var AutorById = (from a in _bibliotecaContext.Autor
                             join l in _bibliotecaContext.Libro
                             on a.Id equals l.AutorId
                             where a.Id == id
                             select new
                             {
                                 a.Id,
                                 a.Nombre,
                                 a.Nacionalidad,
                                 l.Titulo

                             }).ToList();

            if (AutorById.Count() == 0)
            {
                return NotFound();
            }
            return Ok(AutorById);
        }

        [HttpPost]
        [Route("AddAutor")]
        public IActionResult AgregarAutor([FromBody] Autor autor)
        {
            try
            {
                _bibliotecaContext.Autor.Add(autor);
                _bibliotecaContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateAutor")]
        public IActionResult ActualizarAutor(int id, [FromBody] Autor autorModificar)
        {
            Autor? autorActualizado = (from a in _bibliotecaContext.Autor
                                       where a.Id == id
                                       select a).FirstOrDefault();

            if (autorActualizado == null)
            { return NotFound(); }

            autorActualizado.Nombre = autorModificar.Nombre;
            autorActualizado.Nacionalidad = autorModificar.Nacionalidad;

            _bibliotecaContext.Entry(autorActualizado).State = EntityState.Modified;
            _bibliotecaContext.SaveChanges();
            return Ok(autorModificar);
        }

        [HttpDelete]
        [Route("DeleteAutor")]
        public IActionResult EliminarLibro(int id)
        {
            Autor? autor = (from a in _bibliotecaContext.Autor
                            where a.Id == id
                            select a).FirstOrDefault();

            if (autor == null)
                return NotFound();

            _bibliotecaContext.Autor.Attach(autor);
            _bibliotecaContext.Autor.Remove(autor);
            _bibliotecaContext.SaveChanges();
            return Ok(autor);
        }

        /// Endpoints para libros
        /// 
        [HttpGet]
        [Route("GetLibros")]

        public IActionResult GetLibros()
        {
            var listadoLibros = (from l in _bibliotecaContext.Libro
                                 select l).ToList();

            if (listadoLibros.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoLibros);
        }
        [HttpGet]
        [Route("GetIdLibro/{id}")]

        public IActionResult GetLibroId(int id)
        {
            var AutorById = (from l in _bibliotecaContext.Libro
                             join a in _bibliotecaContext.Autor
                             on l.AutorId equals a.Id
                             where l.Id == id
                             select new
                             {
                                 l.Id,
                                 l.Titulo,
                                 l.AñoPublicacion,
                                 l.AutorId,
                                 l.CategoriaId,
                                 l.Resumen,
                                 a.Nombre

                             }).ToList();

            if (AutorById.Count() == 0)
            {
                return NotFound();
            }
            return Ok(AutorById);
        }

        [HttpPost]
        [Route("AddLibro")]
        public IActionResult AgregarLibro([FromBody] Libro libro)
        {
            try
            {
                _bibliotecaContext.Libro.Add(libro);
                _bibliotecaContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateLibro")]
        public IActionResult ActualizarLibro(int id, [FromBody] Libro modificarLibro)
        {
            Libro? LibroCambio = (from l in _bibliotecaContext.Libro
                                  where l.Id == id
                                  select l).FirstOrDefault();

            if (LibroCambio == null)
            { return NotFound(); }

            LibroCambio.Titulo = modificarLibro.Titulo;
            LibroCambio.AñoPublicacion = modificarLibro.AñoPublicacion;
            LibroCambio.AutorId = modificarLibro.AutorId;
            LibroCambio.CategoriaId = modificarLibro.CategoriaId;
            LibroCambio.Resumen = modificarLibro.Resumen;

            _bibliotecaContext.Entry(LibroCambio).State = EntityState.Modified;
            _bibliotecaContext.SaveChanges();
            return Ok(modificarLibro);
        }

        [HttpDelete]
        [Route("DeleteLibro")]
        public IActionResult EliminarAutor(int id)
        {
            Autor? autor = (from a in _bibliotecaContext.Autor
                            where a.Id == id
                            select a).FirstOrDefault();

            if (autor == null)
                return NotFound();

            _bibliotecaContext.Autor.Attach(autor);
            _bibliotecaContext.Autor.Remove(autor);
            _bibliotecaContext.SaveChanges();
            return Ok(autor);
        }

        ///Consultas LINQ 
        ///

        [HttpGet]
        [Route("LibroById")]
        public IActionResult LibrosMayores()
        {
            var listadoLibros = (from l in _bibliotecaContext.Libro
                                 where l.AñoPublicacion > 2000
                                 select l).ToList();

            if (listadoLibros == null)
            {
                return NotFound();
            }
            return Ok(listadoLibros);

        }


        [HttpGet]
        [Route("ContarLibroById")]
        public IActionResult Contar(int id)
        {
            var listadoLibros = (from l in _bibliotecaContext.Libro
                                 where l.AutorId == id
                                 select l).Count();

            if (listadoLibros == 0)
            {
                return NotFound();
            }
            return Ok(listadoLibros);

        }

        [HttpGet]
        [Route("GetTituloLibro/{titulo}")]

        public IActionResult GetLibroTitulo(string titulo)
        {
            var AutorById = (from l in _bibliotecaContext.Libro
                             join a in _bibliotecaContext.Autor
                             on l.AutorId equals a.Id
                             where l.Titulo == titulo
                             select new
                             {
                                 l.Id,
                                 l.Titulo,
                                 l.AñoPublicacion,
                                 l.AutorId,
                                 l.CategoriaId,
                                 l.Resumen,
                                 a.Nombre

                             }).ToList();

            if (AutorById.Count() == 0)
            {
                return NotFound();
            }
            return Ok(AutorById);
        }


        /// Segundas consultas LINQ
        /// 

        [HttpGet]
        [Route("MasLibros")]
        public IActionResult AutoresConMasLibros()
        {
            var listadoslibros = (from l in _bibliotecaContext.Libro
                                       join a in _bibliotecaContext.Autor
                                       on l.AutorId equals a.Id
                                       group l by new { a.Id, a.Nombre } into g
                                       orderby g.Count() descending
                                       select new
                                       {
                                           AutorId = g.Key.Id,
                                           Nombre = g.Key.Nombre,
                                           CantidadLibros = g.Count()
                                       }).Take(5).ToList();

            if (listadoslibros.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoslibros);
        }


        [HttpGet]
        [Route("Recientes")]
        public IActionResult LibrosMasRecientes()
        {
            var librosMasRecientes = (from l in _bibliotecaContext.Libro
                                      orderby l.AñoPublicacion descending
                                      select l)
                                      .ToList();

            return Ok(librosMasRecientes);
        }

        [HttpGet]
        [Route("CantidadLibros")]
        public IActionResult CantidadLibrosPorAño()
        {
            var cantidadLibrosPorAño = (from l in _bibliotecaContext.Libro
                                        group l by l.AñoPublicacion into g
                                        select new
                                        {
                                            Año = g.Key,
                                            CantidadLibros = g.Count()
                                        }).ToList();

            return Ok(cantidadLibrosPorAño);
        }

        [HttpGet]
        [Route("VerAutorLibros/{id}")]
        public IActionResult VerificarAutorLibros(int id)
        {
            var autorConLibros = (from l in _bibliotecaContext.Libro
                                  where l.AutorId == id
                                  select new
                                  {
                                      l.AutorId
                                  }).FirstOrDefault();

            if (autorConLibros == null)
            {
                return NotFound();
            }

            return Ok(autorConLibros);
        }



        [HttpGet]
        [Route("PrimerLibroByAutor/{id}")]
        public IActionResult PrimerLibro(int id)
        {
            var listaLibro = (from l in _bibliotecaContext.Libro
                              where l.AutorId == id
                              orderby l.AñoPublicacion ascending
                              select new
                              {
                                  l.Id,
                                  l.Titulo,
                                  l.AñoPublicacion,
                                  l.AutorId,
                                  l.CategoriaId,
                                  l.Resumen
                              }).FirstOrDefault();

            if (listaLibro == null)
            {
                return NotFound();
            }

            return Ok(listaLibro);
        }
    }

}
