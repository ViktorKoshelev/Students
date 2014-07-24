﻿using System.Collections.Generic;
using FilmService.KindsOfMovies;

namespace FilmService
{
    public class Customer
    {
        public Customer(string name)
        {
            Name = name;
            Rentals = new List<Rental>();
        }

        public string Name
        {
            get; 
            set;
        }

        public List<Rental> Rentals
        {
            get; 
            private set;
        }

        public string Statement()
        {
            double totalAmount = 0;
            int frequentRenterPoints = 0;
            string result = "Учет аренды для " + Name + "\n";
            foreach (var rental in Rentals)
            {
                var thisAmount = rental.Movie.CurrentCalculator.Calculate(rental.DaysRented);

                frequentRenterPoints+=rental.GetPoints();

                result += "\t" + rental.Movie.Title + "\t" + thisAmount + "\n";
                totalAmount += thisAmount;
            }

            result += "Сумма задолженности составляет " + totalAmount + "\n";
            result += "Вы заработали " + frequentRenterPoints + " за активность";
            return result;
        }
    }
}