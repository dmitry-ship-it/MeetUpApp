using AutoMapper;
using MeetUpApp.Data.DAL;
using MeetUpApp.Data.Models;
using MeetUpApp.Resources;
using MeetUpApp.ViewModels;

namespace MeetUpApp.Managers
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

        public async Task<Meetup> GetAsync(int id,
            CancellationToken cancellationToken = default)
        {
            var meetup = await repository.GetByExpressionAsync(
                m => m.Id == id, cancellationToken);

            if (meetup is null)
            {
                throw new ArgumentException(ExceptionMessages.MeetupNotFound);
            }

            return meetup;
        }

        public async Task UpdateAsync(
            int id,
            MeetupViewModel viewModel,
            CancellationToken cancellationToken = default)
        {
            var dbModel = mapper.Map<MeetupViewModel, Meetup>(viewModel);
            mapper.Map(viewModel.Address, dbModel);
            mapper.Map(id, dbModel);

            // check if meetup with this id exists
            // without this check EF throws DbUpdateConcurrencyException
            // which can't be caught
            // throws ArgumentException
            await GetAsync(id, cancellationToken);

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