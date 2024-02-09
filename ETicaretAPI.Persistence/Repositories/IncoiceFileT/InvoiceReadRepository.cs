using ETicaretAPI.Application.Repositories.IncoiceFileT;
using ETicaretAPI.Domain.Entites;
using ETicaretAPI.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Repositories.IncoiceFileT
{
    public class InvoiceReadRepository : ReadRepository<InvoiceFile>, IInvoiceFileReadRepository
    {
        public InvoiceReadRepository(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
