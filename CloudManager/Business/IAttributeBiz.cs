using CloudManager.Common;
using CloudManager.Modeld;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudManager.Business
{
    public interface IAttributeBiz
    {
        Task<CommandResult> Create(List<Attribute> attributes);
        CommandResult Delete(Attribute attribute);
    }
}