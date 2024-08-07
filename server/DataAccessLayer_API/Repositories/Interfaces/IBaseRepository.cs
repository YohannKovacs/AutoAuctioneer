﻿using System.Data;
using DataAccessLayer_AutoAuctioneer.Util;

namespace DataAccessLayer_AutoAuctioneer.Repositories.Interfaces;

public interface IBaseRepository
{
    string connectionStringName { get; set; }

    Task<Result<T>> LoadData<T, U>(string sql, U parameters);
    Task<Result<T>> SaveData<T>(string sql, T parameters, CommandType? cmdType);
}