﻿using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Services
{
    public interface IRepositorioCategorias
    {
        Task Crear(Categoria categoria);
        Task Eliminar(int id);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId);
        Task<Categoria> ObtenerPorId(int id, int usuarioId);
    }

    public class RepositorioCategorias : IRepositorioCategorias
    {
        private readonly string connectionString;
        public RepositorioCategorias(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Categoria> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(this.connectionString);
            var query = "SELECT * FROM Categorias WHERE Id = @Id AND UsuarioId = @UsuarioId";
            return await connection.QueryFirstOrDefaultAsync<Categoria>(query, new { id, usuarioId });
        }

        public async Task Crear(Categoria categoria)
        {
            using var connection = new SqlConnection(this.connectionString);
            var query = "INSERT INTO Categorias(Nombre, TipoOperacionId, UsuarioId) " +
                "VALUES(@Nombre, @TipoOperacionId, @UsuarioId);SELECT SCOPE_IDENTITY();";
            var id = await connection.QuerySingleAsync<int>(query, categoria);
            categoria.Id = id;
        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId )
        {
            using var connection = new SqlConnection(this.connectionString);
            var query = "SELECT * FROM Categorias WHERE UsuarioId = @UsuarioId";
            return await connection.QueryAsync<Categoria>(query, new { usuarioId });
        }

        public async Task Actualizar(Categoria categoria)
        {
            using var connection = new SqlConnection(this.connectionString);
            var query = "UPDATE Categorias SET Nombre = @Nombre, TipoOperacionId = @TipoOperacionId " +
                "WHERE Id = @Id";
            await connection.ExecuteAsync(query, categoria);
        }

        public async Task Eliminar(int id)
        {
            using var connection = new SqlConnection(this.connectionString);
            var query = "DELETE FROM Categorias WHERE Id = @Id";
            await connection.ExecuteAsync(query, new { id });
        }
    }
}
