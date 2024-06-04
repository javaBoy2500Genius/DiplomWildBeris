using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWildBeris.Models
{
    public class Topic : EmbeddedObject
    {
        public string? TopicName { get; set; }
        public string? TopicDescription { get; set; }

    }
}
