using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
namespace Business.Validation
{
    [Serializable]
    public class MarketException:Exception
    {
        public MarketException(string message = "MarketException") : base(message)
        {

        }

        public MarketException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MarketException(SerializationInfo info, StreamingContext context)
           : base(info, context)
        {
        }
    }
}
