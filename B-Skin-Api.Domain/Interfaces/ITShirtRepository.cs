﻿using B_Skin_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_Skin_Api.Domain.Interfaces
{
    public interface ITShirtRepository
    {
        Task<IEnumerable<TShirtModel>> GetAll(bool onlyActives);
        Task<TShirtModel> GetById(long id, bool onlyActives);
        Task InactivateById(long id);
        Task ReactivateById(long id);
        Task<TShirtModel> Create(TShirtModel entity);
        Task Update(long id, TShirtModel entity);
    }
}
