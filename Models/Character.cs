using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DragonBallApi.Models
{
    public class Character
    {
        public int Id { get; set; }

        [MaxLength(25)]
        public string Name { get; set; }

        [MaxLength(35)]
        public string Ki { get; set; }

        [MaxLength(25)]
        public string Race { get; set; }

        [MaxLength(20)]
        public string Gender { get; set; }

        public string Description { get; set; }

        [MaxLength(35)]
        public string Affiliation { get; set; }

        public ICollection<Transformation> Transformations { get; set; }
    }
}
