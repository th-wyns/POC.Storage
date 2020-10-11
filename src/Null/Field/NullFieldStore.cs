using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace POC.Storage.Null
{
    class NullFieldStore : FieldStoreBase
    {
        public ConcurrentBag<Field> FieldsList { get; }

        public NullFieldStore()
        {
            FieldsList = new ConcurrentBag<Field>();
        }

        public async override Task CreateAsync(Field field, CancellationToken cancellationToken)
        {
            Trace.WriteLine("NullFieldStore.CreateAsync");
            await Task.Yield();
            FieldsList.Add(field);
        }

        public override Task DeleteAsync(Field field, CancellationToken cancellationToken)
        {
            Trace.WriteLine("NullFieldStore.Delete");
            return default!;
        }

        public async override Task<IList<Field>> FindAllAsync(CancellationToken cancellationToken)
        {
            Trace.WriteLine("NullFieldStore.FindAll");

            await Task.Yield();
            return FieldsList.ToList();
        }

        public async override Task<Field> FindByIdAsync(string id, CancellationToken cancellationToken)
        {
            Trace.WriteLine("NullFieldStore.FindById");
            await Task.Yield();
            return FieldsList.FirstOrDefault(f => f.Id == id);
        }

        public async override Task<Field> FindByNameAsync(string name, CancellationToken cancellationToken)
        {
            Trace.WriteLine("NullFieldStore.FindByName");
            await Task.Yield();
            return FieldsList.FirstOrDefault(f => f.Name == name);
        }

        public override Task UpdateAsync(Field field, CancellationToken cancellationToken)
        {
            Trace.WriteLine("NullFieldStore.Update");
            return default!;
        }
    }
}
