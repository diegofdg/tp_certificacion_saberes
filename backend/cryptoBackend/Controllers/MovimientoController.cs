using cryptoBackend.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;

namespace cryptoBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class MovimientoController : ApiController
    {
        cryptomarcaEntities db = new cryptomarcaEntities();

        [HttpGet]
        [Route("api/Movimiento/getMovimientoUsuario/{idUsuario:int}")]
        public IHttpActionResult Get(Int64 idUsuario)
        {
            if (idUsuario <= 0) return BadRequest("Parametro idUsuario Incorrecto.");

            try
            {
                var lmovimientos =
                            (
                           db.billeterasMonedas.Where(xx => xx.fk_usuario == idUsuario)
                           .Join(db.movimientos, xx => xx.id, mo => mo.fk_billeteraMoneda_Destino, (xx, mo) => new { mo.id, mo.fecha, mo.fk_billeteraMoneda_Origen, mo.cantidad_Origen, mo.fk_billeteraMoneda_Destino, mo.cantidad_Destino, mo.fk_tipoMovimiento })
                           )
                            .Concat(
                                (
                                db.billeterasMonedas.Where(xx => xx.fk_usuario == idUsuario)
                                .Join(db.movimientos, xx => xx.id, mo => mo.fk_billeteraMoneda_Origen, (xx, mo) => new { mo.id, mo.fecha, mo.fk_billeteraMoneda_Origen, mo.cantidad_Origen, mo.fk_billeteraMoneda_Destino, mo.cantidad_Destino, mo.fk_tipoMovimiento })
                                )
                            )
                            .Join(db.tipoMovimientos, mo => mo.fk_tipoMovimiento, tt => tt.id, (mo, tt) => new { mo.id, mo.fecha, mo.fk_billeteraMoneda_Origen, mo.cantidad_Origen, mo.fk_billeteraMoneda_Destino, mo.cantidad_Destino, mo.fk_tipoMovimiento ,tipoMovimiento = tt.descripcion })
                           .OrderBy(xx=>xx.id)
                           .Distinct();

                List<object> listado = new List<object>();
                foreach(var obj in lmovimientos)
                {
                    var monedaDestino = db.billeterasMonedas.Where(xx => xx.id == obj.fk_billeteraMoneda_Destino).Join(db.monedas, xx => xx.fk_moneda, mo => mo.id, (xx, mo) => new { mo.id, nombre = mo.nombre }).First();
                    var monedaOrigen = monedaDestino;

                    if (obj.fk_billeteraMoneda_Origen != null)
                    {
                        monedaOrigen = db.billeterasMonedas.Where(xx => xx.id == obj.fk_billeteraMoneda_Origen).Join(db.monedas, xx => xx.fk_moneda, mo => mo.id, (xx, mo) => new { mo.id, nombre = mo.nombre }).FirstOrDefault();
                    }

                    object dyn = new
                    {
                        id = obj.id,
                        fecha = obj.fecha,
                        monedaOrigen = (obj.fk_billeteraMoneda_Origen != null ? monedaOrigen.nombre.ToString() : null),
                        cantidadOrigen = obj.cantidad_Origen,
                        monedaDestino = monedaDestino.nombre.ToString(),
                        cantidadDestino = obj.cantidad_Destino,
                        tipoMovimiento=obj.tipoMovimiento
                    };

                    listado.Add(dyn);
                };

                return Ok(listado);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        // POST: api/Movimiento
        public IHttpActionResult Post([FromBody] movimientos movimiento)
        {
            if ((movimiento.fk_billeteraMoneda_Origen < 0)) return NotFound();
            if (movimiento.fk_billeteraMoneda_Destino <= 0) return NotFound();
            if (movimiento.fk_tipoMovimiento <= 0) return NotFound();

            try
            {
                using (db = new cryptomarcaEntities())
                {
                    movimientos newMovimiento = new movimientos()
                    {
                        fk_billeteraMoneda_Origen = (movimiento.fk_billeteraMoneda_Origen!=0)?movimiento.fk_billeteraMoneda_Origen:null,
                        fk_billeteraMoneda_Destino = movimiento.fk_billeteraMoneda_Destino,
                        cantidad_Origen = (movimiento.cantidad_Origen!=0)?movimiento.cantidad_Origen:null,
                        cantidad_Destino = movimiento.cantidad_Destino,
                        fecha = DateTime.Now,
                        fk_tipoMovimiento = movimiento.fk_tipoMovimiento
                    };

                    db.movimientos.Add(newMovimiento);
                    db.SaveChanges();

                    return Created("Creado", newMovimiento);
                }
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
