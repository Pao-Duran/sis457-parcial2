using CadParcial2Pcdh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace ClnParcial2Pcdh
{
    public class SerieCln
    {
        public static int insertar(Serie serie)
        {
            using (var context = new Parcial2PcdhEntities1())
            {
                context.Serie.Add(serie);
                context.SaveChanges();
                return serie.id;
            }
        }

        public static int actualizar(Serie serie)
        {
            using (var context = new Parcial2PcdhEntities1())
            {
                var existente = context.Serie.Find(serie.id);
                existente.titulo = serie.titulo;
                existente.sinopsis = serie.sinopsis;
                existente.director = serie.director;
                existente.duracion = serie.duracion;
                existente.fechaEstreno = serie.fechaEstreno;
                existente.usuarioRegistro = serie.usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id, string usuarioRegistro)
        {
            using (var context = new Parcial2PcdhEntities1())
            {
                var existente = context.Serie.Find(id);
                existente.estado = -1;
                existente.usuarioRegistro = usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static Serie get(int id)
        {
            using (var context = new Parcial2PcdhEntities1())
            {
                return context.Serie.Find(id);
            }
        }

        public static List<paSerieListar_Result> listarPa(string parametro)
        {
            using (var context = new Parcial2PcdhEntities1())
            {
                return context.paSerieListar(parametro).ToList();
            }
        }
    }
}
