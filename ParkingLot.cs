using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public enum VehicleSize { Motorcycle, Compact, Large }

    public abstract class Vehicle
    {
        public abstract VehicleSize Size { get; }
        public abstract int NumberOfSpots { get;  }

        public List<ParkingSpot> ParkingSpots = new List<ParkingSpot>();

        public void ParkInSpot(ParkingSpot spot) { 
            ParkingSpots.Add(spot);
        }

        public void ClearSpots() {
            foreach (var spot in ParkingSpots) { 
                spot.RemoveVehicle();
            }

            ParkingSpots.Clear();
        }

        public abstract bool CanFitInSpot(ParkingSpot spot);

    }

    public class MotorCycle : Vehicle
    {
        public override VehicleSize Size => VehicleSize.Motorcycle;
        public override int NumberOfSpots => 1;
        public override bool CanFitInSpot(ParkingSpot spot) => true;
    }

    public class Car : Vehicle
    {
        public override VehicleSize Size => VehicleSize.Compact;
        public override int NumberOfSpots => 1;
        public override bool CanFitInSpot(ParkingSpot spot) => spot.SpotSize == VehicleSize.Compact 
                                                            || spot.SpotSize == VehicleSize.Large;
    }

    public class Bus : Vehicle
    {
        public override VehicleSize Size => VehicleSize.Large;
        public override int NumberOfSpots => 5;
        public override bool CanFitInSpot(ParkingSpot spot) => spot.SpotSize == VehicleSize.Large;
    }

    public class ParkingSpot {
        public VehicleSize SpotSize { get; private set; }
        public int Row { get; private set; }
        public int SpotNumber { get; private set; }
        public Level Level { get; private set; }
        public Vehicle CurrentVehicle { get; private set; }

        public ParkingSpot(VehicleSize size, int row, int spotNumber, Level level)
        {
            SpotNumber = spotNumber;
            Row = row;
            SpotSize = size;
            Level = level;
        }

        public bool IsAvailable => CurrentVehicle == null;

        public bool Fit(Vehicle vehicle) => IsAvailable && vehicle.CanFitInSpot(this);

        public bool Park(Vehicle vehicle)
        {
            if (!Fit(vehicle)) return false;

            CurrentVehicle = vehicle;
            vehicle.ParkInSpot(this);

            return true;
        }

        public void RemoveVehicle() {
            CurrentVehicle = null;
        }
    }

    public class Level {
        private List<ParkingSpot> spots;
        private int Rows;
        private int SpotsPerRow;

        public Level(int floor, int rows, int spotsPerRow)
        {
            Rows = rows;
            SpotsPerRow = spotsPerRow;
            spots = new List<ParkingSpot>();
            GenerateSpots();
        }

        private void GenerateSpots() {
            for (int row = 0; row < Rows; row++) { 
                for(int j = 0; j < SpotsPerRow; j++)
                {
                    var spotSize = (j < SpotsPerRow / 4) ? VehicleSize.Motorcycle : 
                                   (j < SpotsPerRow * 3/4) ? VehicleSize.Compact : VehicleSize.Large;

                    spots.Add(new ParkingSpot(spotSize, row, j, this));
                }
            }
        }

        public bool ParkVehicle(Vehicle vehicle)
        {
            var spotsNeeded = vehicle.NumberOfSpots;

            for (int i = 0; i < spots.Count; i++) {
                if (CanFit(vehicle, i)) {
                    for (int j = 0; j < SpotsPerRow; j++)
                    {
                        spots[i + j].Park(vehicle);
                    }

                    return true;
                }
            }

            return false;
        }

        private bool CanFit(Vehicle vehicle, int index) {
            var spotsNeeded = vehicle.NumberOfSpots;
            if(spotsNeeded + index > spots.Count) return false;

            var row = spots[index].Row;

            for(int j = 0;j < SpotsPerRow; j++)
            {
                var spot = spots[index + j];
                if (row != spot.Row || spot.Fit(vehicle)) return false;
            }

            return true;
        }
    }

    public class ParkingLot
    {
        private List<Level> levels;

        public ParkingLot(int numOfLevels, int rowsPerLevel, int spotsPerRow)
        {
            levels = new List<Level>();
            
            for (int i = 0; i < numOfLevels; i++)
            {
                levels.Add(new Level(i, rowsPerLevel, spotsPerRow));
            }
        }

        public bool ParkVehicle(Vehicle vehicle)
        {
            foreach(var level in levels)
            {
                if(level.ParkVehicle(vehicle)) return true;
            }

            return false;
        }
    }
}
