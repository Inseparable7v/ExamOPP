using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using AquaShop.Models.Fish;
using AquaShop.Repositories;
using AquaShop.Core.Contracts;
using AquaShop.Models.Aquariums;
using AquaShop.Models.Decorations;
using AquaShop.Utilities.Messages;
using AquaShop.Models.Aquariums.Contracts;
using AquaShop.Models.Decorations.Contracts;

namespace AquaShop.Core
{
    public class Controller : IController
    {
        private DecorationRepository decorations;
        private ICollection<IAquarium> aquariums;
        public Controller()
        {
            this.decorations = new DecorationRepository();
            this.aquariums = new List<IAquarium>();
        }
        public string AddAquarium(string aquariumType, string aquariumName)
        {
            var message = string.Empty;
            Aquarium aquarium = null;

            if (aquariumType == "FreshwaterAquarium")
            {
                aquarium = new FreshwaterAquarium(aquariumName);
                message = string.Format(OutputMessages.SuccessfullyAdded, aquariumType);
                this.aquariums.Add(aquarium);
            }
            else if (aquariumType == "SaltwaterAquarium")
            {
                aquarium = new SaltwaterAquarium(aquariumName);
                message = string.Format(OutputMessages.SuccessfullyAdded, aquariumType);
                this.aquariums.Add(aquarium);
            }
            else
            {
                throw new InvalidOperationException(ExceptionMessages.InvalidAquariumType);
            }

            return message;
        }

        public string AddDecoration(string decorationType)
        {
            var message = string.Empty;
            if (decorationType == "Ornament")
            {
                message = string.Format(OutputMessages.SuccessfullyAdded, decorationType);
                var decoration = new Ornament();
                this.decorations.Add(decoration);
            }
            else if (decorationType == "Plant")
            {
                message = string.Format(OutputMessages.SuccessfullyAdded, decorationType);
                var decoration = new Plant();
                this.decorations.Add(decoration);
            }
            else
            {
                throw new InvalidOperationException(ExceptionMessages.InvalidDecorationType);
            }

            return message;
        }

        public string InsertDecoration(string aquariumName, string decorationType)
        {
            var decoration = this.decorations.FindByType(decorationType);

            var message = string.Empty;
            if (decoration != null)
            {
                var aquarium = this.aquariums.FirstOrDefault(a => a.Name.Equals(aquariumName));
                aquarium.AddDecoration(decoration);
                this.decorations.Remove(decoration);
                message = string.Format(OutputMessages.EntityAddedToAquarium, decorationType, aquariumName);
            }
            else
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.InexistentDecoration, decorationType));
            }

            return message;
        }

        public string AddFish(string aquariumName, string fishType, string fishName, string fishSpecies, decimal price)
        {
            var aquarium = this.aquariums.FirstOrDefault(a => a.Name.Equals(aquariumName));
            Fish fish = null;
            var message = string.Empty;
            if (fishType == "FreshwaterFish")
            {
                fish = new FreshwaterFish(fishName, fishSpecies, price);
            }
            else if (fishType == "SaltwaterFish")
            {
                fish = new SaltwaterFish(fishName, fishSpecies, price);
            }
            else
            {
                throw new InvalidOperationException(ExceptionMessages.InvalidFishType);
            }

            if (aquarium != null)
            {
                if (fishType == "SaltwaterFish" && aquarium.GetType().Name == "SaltwaterAquarium" || fishType == "FreshwaterFish" && aquarium.GetType().Name == "FreshwaterAquarium")
                {
                    aquarium.AddFish(fish);
                    message = string.Format(OutputMessages.EntityAddedToAquarium, fishType, aquariumName);
                }
                else
                {
                    message = OutputMessages.UnsuitableWater;
                }
            }

            return message;
        }

        public string FeedFish(string aquariumName)
        {
            var aquarium = this.aquariums.FirstOrDefault(a => a.Name.Equals(aquariumName));
            for (int i = 0; i < aquarium.Fish.Count; i++)
            {
                aquarium.Feed();
            }

            return string.Format(OutputMessages.FishFed, aquarium.Fish.Count);
        }

        public string CalculateValue(string aquariumName)
        {
            var aquarium = this.aquariums.FirstOrDefault(a => a.Name.Equals(aquariumName));

            var fishPrice = aquarium.Fish.Sum(f => f.Price);
            var decorationPrice = aquarium.Decorations.Sum(d => d.Price);

            var value = fishPrice + decorationPrice;

            return string.Format(OutputMessages.AquariumValue, aquariumName, value);
        }

        public string Report()
        {
            var sb = new StringBuilder();
            foreach (var aquarium in aquariums)
            {
                sb.AppendLine(aquarium.GetInfo());
            }
            return sb.ToString().TrimEnd();
        }
    }
}
