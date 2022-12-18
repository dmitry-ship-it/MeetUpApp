using AutoMapper;
using MeetUpApp.Api.Data.DAL;
using MeetUpApp.Api.Data.Models;
using MeetUpApp.Api.ViewModels;

namespace MeetUpApp.Api.Managers
{
    public class MeetupManager
    {
        private readonly IRepository<Meetup> repository;
        private readonly IMapper mapper;

        public MeetupManager(IRepository<Meetup> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Meetup>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await repository.GetAllAsync(cancellationToken);
        }

        public async Task AddAsync(
            MeetupViewModel viewModel,
            CancellationToken cancellationToken = default)
        {
            var dbModel = mapper.Map<MeetupViewModel, Meetup>(viewModel);
            mapper.Map(viewModel.Address, dbModel);
            await repository.InsertAsync(dbModel, cancellationToken);
        }

        public async Task<Meetup?> GetAsync(int id,
            CancellationToken cancellationToken = default)
        {
            return await repository.GetByIdAsync(id, cancellationToken);
        }

        public async Task UpdateAsync(
            int id,
            MeetupViewModel viewModel,
            CancellationToken cancellationToken = default)
        {
            var dbModel = mapper.Map<MeetupViewModel, Meetup>(viewModel);
            mapper.Map(viewModel.Address, dbModel);
            dbModel.Id = id;

            await repository.UpdateAsync(dbModel, cancellationToken);
        }

        public async Task RemoveAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var meetup = await GetAsync(id, cancellationToken);
            ArgumentNullException.ThrowIfNull(meetup, nameof(id));

            await repository.RemoveAsync(meetup, cancellationToken);
        }
    }
}
