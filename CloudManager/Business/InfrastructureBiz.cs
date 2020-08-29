using CloudManager.Common;
using CloudManager.Common.Helper;
using CloudManager.Modeld;
using CloudManager.Properties;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudManager.Business
{
    public class InfrastructureBiz : IInfrastructureBiz
    {
        private IResourceBiz _resourceBiz;
        private IFileHelper _fileHelper;
        public InfrastructureBiz(IResourceBiz resourceBiz, IFileHelper fileHelper)
        {
            _resourceBiz = resourceBiz;
            _fileHelper = fileHelper;
        }
        public async Task<CommandResult> Create(List<Infrastructure> infrastructures)
        {
            var result = new CommandResult();
            foreach (var infrastructure in infrastructures)
            {
                _fileHelper.CreateDirectory(infrastructure);
                var messages = await _resourceBiz.Create(infrastructure.Resources);
                result.ResultMessages.AddRange(messages.ResultMessages);
                result.ResultMessages.Add(new ResultMessage()
                {
                    IsSuccessed = true,
                    Message = string.Format(Resources.Create, Infrastructure.GetName() + " " + infrastructure.Name)
                });
            }
            return result;
        }

        public CommandResult Delete(List<Infrastructure> infrastructures)
        {
            var result = new CommandResult();
            foreach (var infrastructure in infrastructures)
            {
                var directories = _fileHelper.GetDirectories(infrastructure);
                infrastructure.Resources = directories.Select(dir => {
                   var res = new Resource() { Name = dir.Name, Infrastructure = infrastructure };
                    res.Attributes = new List<Attribute>() { new Attribute() { Resource = res } };
                    return res;
                }).ToList();
              
                result.ResultMessages.AddRange(_resourceBiz.Delete(infrastructure.Resources).ResultMessages);

                result.ResultMessages.Add(new ResultMessage()
                {
                    IsSuccessed = _fileHelper.DeleteDirectory(infrastructure),
                    Message = string.Format(Resources.Delete, Infrastructure.GetName() + " " + infrastructure.Name)
                });
            }

            return result;
        }
    }
}
