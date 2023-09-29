﻿using ecommerce_iniciativatena.Data;
using ecommerce_iniciativatena.Model;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_iniciativatena.Service.Implements
{
    public class CategoriaService : ICategoriaService
    {
        private readonly AppDbContext _context;

        public CategoriaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> GetAll()
        {
            return await _context.Categorias
                //.Include(t => t.Produto)
                .ToListAsync();

        }

        public async Task<Categoria?> GetById(long id)
        {
            try
            {

                var Categoria = await _context.Categorias
                    //.Include(t => t.Produto)
                    .FirstAsync(i => i.Id == id);

                return Categoria;

            }
            catch
            {
                return null;
            }

        }

        public async Task<IEnumerable<Categoria>> GetByNome(string nome)
        {
            var Categoria = await _context.Categorias
                            //.Include(t => t.Produto)
                            .Where(c => c.Nome.Contains(nome))
                            .ToListAsync();

            return Categoria;
        }

        public async Task<Categoria?> Create(Categoria categoria)
        {
            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();

            return categoria;
        }

        public async Task<Categoria?> Update(Categoria categoria)
        {
            var CategoriaUpdate = await _context.Categorias.FindAsync(categoria);

            if (CategoriaUpdate is null)
                return null;

            _context.Entry(CategoriaUpdate).State = EntityState.Detached;
            _context.Entry(categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return categoria;

        }

        public async Task Delete(Categoria categoria)
        {
            _context.Remove(categoria);
            await _context.SaveChangesAsync();

        }
    }
}