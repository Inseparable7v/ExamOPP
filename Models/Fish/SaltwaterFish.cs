using System;
using System.Collections.Generic;
using System.Text;

namespace AquaShop.Models.Fish
{
    public class SaltwaterFish : Fish
    {
        private const int _Size = 5;
        private const int _Feed = 2;
        public SaltwaterFish(string name, string species, decimal price) 
            : base(name, species, price)
        {
            this.Size = _Size;
        }

        public override void Eat()
        {
            this.Size += _Feed;
        }
    }
}
