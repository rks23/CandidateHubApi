using CandidateHubApi.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CandidateHubApi.Repository
{
    public class CandidateRepository : IRepository<Candidate>
    {
        private readonly CandidateHubContext _context;

        public CandidateRepository(CandidateHubContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Candidate entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Candidate entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            var ct = GetByIdAsync(Id);
            if (ct is null) return false;
            _context.Remove(ct);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Candidate>> GetByFilterAsync(Expression<Func<Candidate, bool>> expression)
        {
            var result = await _context.Candidates.Where(expression).ToListAsync();
            return result;
        }

        public async Task<Candidate?> GetByIdAsync(int Id)
        {
            var result = await _context.Candidates.FirstOrDefaultAsync(x => x.CandidateId == Id);
            return result;
        }


        public async Task<IEnumerable<Candidate>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 10) pageSize = 10;

            var result = _context.Candidates
                .OrderBy(x => x.CandidateId)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return await result;
        }

        public IQueryable<Candidate> GetByFilterQuery(Expression<Func<Candidate, bool>> expression)
        {
            var query = _context.Candidates.Where(expression);
            return query;
        }
    }
}
