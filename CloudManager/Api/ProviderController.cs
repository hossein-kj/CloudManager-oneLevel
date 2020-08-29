using CloudManager.Business;
using CloudManager.Common;
using CloudManager.Common.Exceptions;
using CloudManager.Common.Model;
using CloudManager.Modeld;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudManager.Api
{
    public class ProviderController
    {
        private IProviderBiz _providerBiz;
        private IInfrastructureBiz _infrastructureBiz;
        public ProviderController(IProviderBiz providerBiz,IInfrastructureBiz infrastructureBiz)
        {
            _providerBiz = providerBiz;
            _infrastructureBiz = infrastructureBiz;
        }
        public async Task<CommandResult> Create(JObject data)
        {
            dynamic providerDto = data;
            JArray attributeArray = providerDto.Attributes;
            var attributes = attributeArray.ToObject<List<KeyValue>>();

            string providerName = providerDto.Name;

            if (string.IsNullOrEmpty(providerName))
                providerName = "AWS";

            string infrastructureName = providerDto.InfrastructureName;

            if (string.IsNullOrEmpty(infrastructureName))
                throw new ValidateException("Infrastructure Name");

            string resourceName = providerDto.ResourceName;


            if (string.IsNullOrEmpty(resourceName))
                throw new ValidateException("Resource Name");

            var resource = new Resource() { Name = resourceName };
            var infrastructure = new Infrastructure() { Name = infrastructureName,
                Resources = new List<Resource>()
                        {
                           resource

                        }
            };

            var provider = new Provider()
            {
                Name = providerName,
                infrastructures = new List<Infrastructure>()
                {
                    infrastructure
                }
            };
            var newAttr = attributes.Select(att => new Modeld.Attribute() { Name = att.Key, Value = att.Value,Resource= resource }).ToList();

            resource.Attributes = newAttr;
            infrastructure.Provider = provider;
            resource.Infrastructure = infrastructure;

            return await _providerBiz.Create(provider);
        }

        public CommandResult Delete(JObject data)
        {
            dynamic providerDto = data;

            string providerName = providerDto.Name;

            if (string.IsNullOrEmpty(providerName))
                providerName = "AWS";

            string infrastructureName = providerDto.InfrastructureName;

            if (string.IsNullOrEmpty(infrastructureName))
                throw new ValidateException("Infrastructure Name");

          
            var infrastructure = new Infrastructure()
            {
                Name = infrastructureName,
            };

            var provider = new Provider()
            {
                Name = providerName,
                infrastructures = new List<Infrastructure>()
                {
                    infrastructure
                }
            };

            infrastructure.Provider = provider;

            return _infrastructureBiz.Delete(provider.infrastructures);
        }
    }
}
