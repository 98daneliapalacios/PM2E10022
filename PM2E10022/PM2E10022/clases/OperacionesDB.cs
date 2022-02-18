using PM2E10022.model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PM2E10022.clases
{
    class OperacionesDB
    {
        cnx conn = new cnx();



        public Task<List<Modelo>> getReadUbicacion()
        {
            return conn.GetConnectionAsync().Table<Modelo>().ToListAsync();
        }

        public Task<Modelo> getUbicacionId(int id)
        {
            return conn
                .GetConnectionAsync()
                .Table<Modelo>()
                .Where(item => item.id == id)
                .FirstOrDefaultAsync();
        }

        public Task<int> getUbicacionUpdateId(Modelo ubicacion)
        {
            return conn
                .GetConnectionAsync()
                .UpdateAsync(ubicacion);

        }

        public Task<int> Delete(Modelo ubicacion)
        {
            return conn
                .GetConnectionAsync()
                .DeleteAsync(ubicacion);
        }
    }
}