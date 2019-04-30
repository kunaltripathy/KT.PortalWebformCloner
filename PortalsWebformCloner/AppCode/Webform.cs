using Microsoft.Xrm.Sdk;

namespace KT.PortalsWebformCloner.AppCode
{ 

    public class Webform
    {
        public Webform(Entity record)
        {
            Record = record;
        }

        public Entity Record { get; }

        public override string ToString()
        {
            return $"{Record.GetAttributeValue<string>("adx_name")} ({Record.Id})";
        }
    }
}
