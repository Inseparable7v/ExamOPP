using System;
using System.Collections.Generic;
using System.Text;

namespace AquaShop.Models.Fish
{
    public class FreshwaterFish : Fish
    {
        private const int _Size = 3;
        private const int _Feed = 3;
        public FreshwaterFish(string name, string species, decimal price) 
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
