using CloudManager.Common;
using CloudManager.Common.Helper;
using CloudManager.Common.Model;
using CloudManager.Properties;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attribute = CloudManager.Modeld.Attribute;

namespace CloudManager.Business
{
    public class AttributeBiz : IAttributeBiz
    {
        private IFileHelper _fileHelper;
        private ISerializeHelper _serializeHelper;
        public AttributeBiz(IFileHelper fileHelper, ISerializeHelper serializeHelper)
        {
            _fileHelper = fileHelper;
            _serializeHelper = serializeHelper;
        }
        public async Task<CommandResult> Create(List<Attribute> attributes)
        {
            var oldAttributes = new List<KeyValue>();
            var commandResult = new CommandResult() { ResultMessages = new List<ResultMessage>() };

            if (attributes.Count == 0)
            {
                commandResult.ResultMessages.Add(new ResultMessage() { IsSuccessed = true, Message = string.Format(Resources.NotFound,Attribute.GetName()) });
                return commandResult;
            }


            if (_fileHelper.FileExist(attributes[0]))
            {
                 oldAttributes = _serializeHelper.Deserialize<List<KeyValue>>(await _fileHelper.ReadAsync(attributes[0]));
            }

            foreach (var attribute in attributes)
            {
                var oldAttribute = oldAttributes.FirstOrDefault(at => at.Key == attribute.Name);

                if (oldAttribute == null)
                {
                    oldAttributes.Add(new KeyValue() { Key= attribute.Name,Value=attribute.Value});
                }
                else
                {
                    oldAttribute.Value = attribute.Value;
                }

                commandResult.ResultMessages.Add(new ResultMessage() { IsSuccessed = true, Message = string.Format(Resources.Create,Attribute.GetName() + " " + attribute.Name) });
            }
            await _fileHelper.WriteAsync(attributes[0], _serializeHelper.Serialize(oldAttributes));

            return commandResult;
        }

        public CommandResult Delete(Attribute attribute)
        {
            return new CommandResult()
            {
                ResultMessages = new List<ResultMessage>() {
                new ResultMessage() { IsSuccessed = _fileHelper.DeleteFile(attribute), Message = string.Format(Resources.Delete,"attribute file") } }
            };
        }
    }
}
