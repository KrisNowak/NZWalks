using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;


        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }


        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await nZWalksDbContext.WalkDifficulty.ToListAsync();
        }


        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walk)
        {
            walk.Id = Guid.NewGuid();

            await nZWalksDbContext.AddAsync(walk);

            await nZWalksDbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            WalkDifficulty walk = await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);

            if (walk == null)
                return null;


            nZWalksDbContext.WalkDifficulty.Remove(walk);

            await nZWalksDbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walk)
        {
            var existingWalk = await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);

            if (existingWalk == null)
                return null;

            existingWalk.Code = walk.Code;

            await nZWalksDbContext.SaveChangesAsync();

            return existingWalk;
        }
    }
}
