using ETicaretAPI.Application.Repositories.File;
using ETicaretAPI.Domain.Entites;
using ETicaretAPI.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Repositories.File
{
    public class FileWriteRepository : WriteRepository<FileEntity>, IFileWriteRepositpory
    {
        public FileWriteRepository(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
