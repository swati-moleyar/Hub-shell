using System.Threading.Tasks;
using Functional;
using Hub.Shell.External.EntityStore.Resources;

namespace Hub.Shell.External.EntityStore
{
    public interface IEntityStoreServiceClient
    {
        Task<Result<EntityResource, EntityStoreError>> GetEntity(int entityId);
    }
}