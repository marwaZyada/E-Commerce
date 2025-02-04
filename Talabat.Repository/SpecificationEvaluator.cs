﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputquery,ISpecification<T> spec)
        {
            var query = inputquery;
            if (spec.Cretiria is not null)
         
                query=query.Where(spec.Cretiria);
            if (spec.OrderBy is not null)

                query = query.OrderBy(spec.OrderBy);

            if (spec.OrderByDescinding is not null)

                query = query.OrderByDescending(spec.OrderByDescinding);

            if (spec.IsPaginationEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            query = spec.Includes.Aggregate(query, (currentquery, incudesexpressions) => currentquery.Include(incudesexpressions));
            return query;
            
        }
    }
}
