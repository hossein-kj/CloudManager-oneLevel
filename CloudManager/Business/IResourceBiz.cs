using CloudManager.Common;
using CloudManager.Modeld;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudManager.Business
{
    public interface IResourceBiz
    {
        Task<CommandResult> Create(List<Resource> resources);
        CommandResult Delete(List<Resource> resources);
    }
}