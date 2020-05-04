using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using AquaShop.Models.Aquariums.Contracts;
using AquaShop.Models.Decorations.Contracts;
using AquaShop.Models.Fish.Contracts;
using AquaShop.Utilities.Messages;

namespace AquaShop.Models.Aquariums
{
    public abstract class Aquarium : IAquarium
    {
        private string name;
        private int comfort;
        private ICollection<IDecoration> decorations;
        private List<IFish> fishes;

        protected Aquarium()
        {
            this.decorations = new List<IDecoration>();
            this.fishes = new List<IFish>();
        }

        protected Aquarium(string name, int capacity)
        : this()
        {
            this.Name = name;
            this.Capacity = capacity;
        }
        public string Name
        {
            get => this.name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.InvalidAquariumName);
                }

                this.name = value;
            }
        }
        public int Capacity { get; private set; }

        public int Comfort
        {
            get => this.decorations.Sum(d => d.Comfort);
        }
        public ICollection<IDecoration> Decorations => this.decorations;
        public ICollection<IFish> Fish => this.fishes;
        public void AddFish(IFish fish)
        {
            if (this.Capacity <= 0)
            {
                throw new InvalidOperationException(ExceptionMessages.NotEnoughCapacity);
            }
            this.fishes.Add(fish);
        }

        public bool RemoveFish(IFish fish)
        {
            return fishes.Remove(fish);
        }

        public void AddDecoration(IDecoration decoration)
        {
            decorations.Add(decoration);
        }

        public void Feed()
        {
            foreach (var fish in fishes)
            {
                fish.Eat();
            }
        }

        public virtual string GetInfo()
        {
            var sb = new StringBuilder();
            var fishesNames = new List<string>();
            foreach (var fish in Fish)
            {
                var fishName = fish.Name;
                fishesNames.Add(fishName);

            }
            if (this.fishes.Count > 0)
            {
                sb.AppendLine($"{this.Name} ({this.GetType().Name}):").AppendLine($"Fish: {string.Join(", ", fishesNames)}").AppendLine($"Decorations: {this.Decorations.Count}")
                    .AppendLine($"Comfort: {this.Comfort}");
            }
            else if (this.fishes.Count <= 0)
            {
                sb.AppendLine($"{this.Name} ({this.GetType().Name}):").AppendLine("Fish: none").AppendLine($"Decorations: {this.Decorations.Count}")
                    .AppendLine($"Comfort: {this.Comfort}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
