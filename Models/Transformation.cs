using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DragonBallApi.Models
{
    public class Transformation
    {
        public int Id { get; set; }

        [MaxLength(25)]
        public string Name { get; set; }

        [MaxLength(35)]
        public string Ki { get; set; }

        // Foreign Key (opcional si EF lo detecta automáticamente)
        public int CharacterId { get; set; }
        public Character Character { get; set; }
    }
}
