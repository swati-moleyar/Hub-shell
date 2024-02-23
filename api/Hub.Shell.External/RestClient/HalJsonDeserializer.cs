using System.Collections.Generic;
using IQ.RestClient.Core;
using IQ.RestClient.Core.Common;

namespace Hub.Shell.External.RestClient
{
    public class HalJsonDeserializer : IDeserializer
    {
        public IEnumerable<MediaType> SupportedMediaTypes => new[] { new MediaType("application/hal+json") };

        public Option<T> Deserialize<T>(ResponseContent source)
        {
            return Option.Some(Newtonsoft.Json.JsonConvert.DeserializeObject<T>(source.StringContent));
        }
    }
}