using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ingesup.Maze.Server.Web.Models
{
    [DataContract]
    public class Player
    {
        public Player()
        {
        }

        [DataMember]
        public string Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Key { get; set; }

        [DataMember]
        public string CreateDate { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Position CurrentPosition { get; set; }

        [DataMember]
        public List<Cell> VisibleCells { get; set; }

        [DataMember]
        public string StartDate { get; set; }

        [DataMember]
        public string FinishDate { get; set; }

        [DataMember]
        public string FinishTime { get; set; }

        [DataMember]
        public int NbMove { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string SecretMessage { get; set; }
    }
}