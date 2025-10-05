using ProjectWeb.API.Data;
using System.Linq.Expressions;

namespace ProjectWeb.API.InterfazGeneral
{
    public class GenericoModelo<Tmodelo> : IGenericoModelo<Tmodelo> where Tmodelo : class
    {
        private readonly AppDbContext _dbContext;
        public GenericoModelo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Tmodelo> CreateReg(Tmodelo modelo)
        {
            try
            {
                _dbContext.Set<Tmodelo>().Add(modelo);
                await _dbContext.SaveChangesAsync();
                return modelo;
            }
            catch
            {
                throw;
            }

        }

        public async Task<bool> Delete(Tmodelo modelo)
        {
            try
            {
                _dbContext.Set<Tmodelo>().Remove(modelo);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public IQueryable<Tmodelo> GetAllWithWhere(Expression<Func<Tmodelo, bool>>? filtro = null)
        {
            IQueryable<Tmodelo> consulta = (filtro == null) ? _dbContext.Set<Tmodelo>() :
                _dbContext.Set<Tmodelo>().Where(filtro);
            return consulta;
        }

        public async Task<bool> Upadate(Tmodelo modelo)
        {
            try
            {
                _dbContext.Set<Tmodelo>().Update(modelo);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public IQueryable<Tmodelo> GetAll()
        {

            return _dbContext.Set<Tmodelo>();
        }
    }
}
