﻿namespace Telegramper.Executors.Routing.UserState
{
    public interface IUserStates
    {
        Task<IEnumerable<string>> GetAsync(long? telegramUserId = null);
        Task SetAsync(string state, long? telegramUserId = null, bool withDefaultState = false);
        Task SetRangeAsync(IEnumerable<string> states, long? telegramUserId = null, bool withDefaultState = false);
        Task AddAsync(string state, long? telegramUserId = null);
        Task RemoveAsync(long? telegramUserId = null);
    }
}