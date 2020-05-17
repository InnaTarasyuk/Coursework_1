using System;
using System.Collections.Generic;
using System.Text;

namespace App.Model
{
    public class Dog
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public double WeightMin { get; set; }
        public double WeightMax { get; set; }
        public int AgeMin { get; set; }
        public int AgeMax { get; set; }
        public string Birthday { get; set; }
        public bool Sex { get; set; }
        public string Gender { get; set; }
        public string Breed { get; set; }
        public string OwnerPhone { get; set; }
        public string OwnerUsername { get; set; }
        public string Avatar { get; set; } = null;
        public override string ToString()
        {
            return $"{Name}, {Breed}, {Weight}, {Gender}, {Birthday}, {OwnerUsername}, {OwnerPhone}, {Avatar}";
        }
    }
   
}
