using Dapper;
using PruebaBirobidBackend.Models;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Transactions;
using System.Xml.Linq;

namespace PruebaBirobidBackend.Business
{
    public class Cliente
    {
        private string _conexion = "Data Source=localhost;Initial Catalog=BD_PRUEBA; user id=cesar; Password=123; MultipleActiveResultSets=true";
        public async Task<List<ClienteModel>> ListarCliente()
        {
            SqlConnection connection = new(_conexion);
            connection.Open();
            var listaAsync = await connection.QueryAsync<ClienteModel>("SpListarClientes", commandType: CommandType.StoredProcedure);
            var lista = listaAsync.ToList();
            connection.Close();
            return lista;
        }
        public async Task<object> GrabarCliente(ClienteModel cliente)
        {
            SqlConnection connection = new(_conexion);
            connection.Open();
            var transaction = connection.BeginTransaction();

            try
            {
                await connection.ExecuteAsync("SpInsertarCliente", 
                    new
                    {
                        cliente.Nombre,
                        cliente.Cedula,
                        cliente.Telefono,
                        cliente.Celular,
                        cliente.Correo,
                        cliente.Genero,
                        cliente.FechaNacimiento
                    }, 
                    transaction, 20000, commandType: CommandType.StoredProcedure);

                transaction.Commit();
                return new { status = 200, mensaje = "Cliente Insertado" };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new { status = 500, mensaje = $"Error al insetar un cliente {ex.Message}" };
            }
            finally
            {
                connection.Close();
            }
           
            
            
        }
    }
}
