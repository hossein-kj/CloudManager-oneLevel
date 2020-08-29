using CloudManager.Common;
using CloudManager.Common.Helper;
using CloudManager.Modeld;
using CloudManager.Properties;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudManager.Business
{
    public class ResourceBiz : IResourceBiz
    {
        private IAttributeBiz _attributeBiz;
        private IFileHelper _fileHelper;

        public ResourceBiz(IAttributeBiz attributeBiz, IFileHelper fileHelper)
        {
            _attributeBiz = attributeBiz;
            _fileHelper = fileHelper;
        }

        public async Task<CommandResult> Create(List<Resource> resources)
        {
            var result = new CommandResult();
            foreach (var resource in resources)
            {
                _fileHelper.CreateDirectory(resource);
                var messages = await _attributeBiz.Create(resource.Attributes);
                result.ResultMessages.AddRange(messages.ResultMessages);
                result.ResultMessages.Add(new ResultMessage() { IsSuccessed = true, Message = string.Format(Resources.Create, Resource.GetName() + " " + resource.Name) });
            }
            return result;
        }

        public CommandResult Delete(List<Resource> resources)
        {
            var result = new CommandResult();
            foreach (var resource in resources)
            {
                if (resource.Attributes.Count > 0)
                    result.ResultMessages.AddRange(_attributeBiz.Delete(resource.Attributes[0]).ResultMessages);

                result.ResultMessages.Add(new ResultMessage()
                {
                    IsSuccessed = _fileHelper.DeleteDirectory(resource),
                    Message = string.Format(Resources.Delete, Resource.GetName() + " " + resource.Name)
                });
            }
            return result;
        }
    }
}
